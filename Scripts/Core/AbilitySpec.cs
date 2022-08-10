using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class AbilitySpec
    {
        #region Events
        public delegate void OnAbilityHandler(AbilitySpec abilitySpec);
        #endregion

        private Ability _Ability;
        private AbilitySystem _Owner;
        private int _Level = 0;
        private bool _isActivate = false;

        public Ability Ability => _Ability;
        public AbilitySystem  Owner => _Owner;
        public GameplayTagSystem GameplayTagSystem => Owner.GameplayTagSystem;
        public GameplayTag AbilityTag => Ability.AbilityTag;
        
        public IReadOnlyCollection<GameplayTag> AttributeTags => Ability.AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => Ability.CancelTags;
        public int Level => _Level;
        public bool Activate => _isActivate;

        public event OnAbilityHandler OnStartedAbility;
        public event OnAbilityHandler OnEndedAbility;
        public event OnAbilityHandler OnFinishedAbility;
        public event OnAbilityHandler OnCanceledAbility;

        public AbilitySpec(Ability ability, AbilitySystem owner, int level)
        {
            _Ability = ability;
            _Owner = owner;
            _Level = level;
        }

        public abstract void OnGrantAbility();
        public abstract void OnLostAbility();

        public bool TryActivateAbility()
        {
            if (Activate)
            {
                if (CanReTriggerAbility())
                {
                    OnReTriggerAbility();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (CanActiveAbility())
                {
                    OnAbility();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void OnReleasedAbility()
        {
            if (!Activate)
                return;

            ReleasedAbility();
        }
        public bool TryReTriggerAbility()
        {
            if (CanReTriggerAbility())
            {
                OnReTriggerAbility();

                return true;
            }

            return false;
        }

        public virtual void OnAbility()
        {
            if (Ability.DebugMode)
                Debug.Log("Enter Ability : " + Ability.name);

            Owner.OnCancelAbility(CancelTags.ToArray());

            _isActivate = true;

            EnterAbility();

            OnStartedAbility?.Invoke(this);
        }

        public void OnReTriggerAbility()
        {
            if (Ability.DebugMode)
                Debug.Log("ReTrigger Ability : " + Ability.name);

            ReTriggerAbility();
        }

        public virtual void EndAbility()
        {
            if (!_isActivate)
                return;

            if (Ability.DebugMode)
                Debug.Log("Exit Ability : " + Ability.name);

            _isActivate = false;

            ExitAbility();

            OnFinishedAbility?.Invoke(this);
            OnEndedAbility?.Invoke(this);
        }

        public virtual void OnCancelAbility()
        {
            if (!_isActivate)
                return;

            if (Ability.DebugMode)
                Debug.Log("Cancel Ability : " + Ability.name);

            _isActivate = false;

            CancelAbility();

            OnCanceledAbility?.Invoke(this);
            OnEndedAbility?.Invoke(this);
        }


        protected abstract void EnterAbility();
        protected abstract void ExitAbility();
        protected abstract void CancelAbility();
        protected abstract void ReleasedAbility();
        protected abstract void ReTriggerAbility();

        public virtual bool CanReTriggerAbility()
        {
            return Activate;
        }

        public virtual bool CanActiveAbility() 
        {
            return false;
        }
    }



}
