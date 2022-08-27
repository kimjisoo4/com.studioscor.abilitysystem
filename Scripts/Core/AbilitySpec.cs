using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;

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

        public bool TryCancelAbility(GameplayTag[] cancelTags)
        {
            if (!Activate)
                return false;

            if (cancelTags.Contains(Ability.AbilityTag))
            {
                OnCancelAbility();

                return true;
            }

            foreach (GameplayTag tag in cancelTags)
            {
                if (AttributeTags.Contains(tag))
                {
                    OnCancelAbility();

                    return true;
                }
            }

            return false;
        }
        public bool TryCancelAbility(IReadOnlyCollection<GameplayTag> cancelTags)
        {
            if (!Activate)
                return false;

            if (cancelTags.Contains(Ability.AbilityTag))
            {
                OnCancelAbility();

                return true;
            }

            foreach (GameplayTag tag in cancelTags)
            {
                if (AttributeTags.Contains(tag))
                {
                    OnCancelAbility();

                    return true;
                }
            }

            return false;
        }

        public virtual void OnAbility()
        {
            Log(" On Ability ");

            Owner.OnCancelAbility(CancelTags.ToArray());

            _isActivate = true;

            EnterAbility();

            OnStartedAbility?.Invoke(this);
        }

        public void OnReTriggerAbility()
        {
            Log(" ReTrigger Ability ");

            ReTriggerAbility();
        }

        public virtual void EndAbility()
        {
            if (!Activate)
                return;

            _isActivate = false;

            
            Log(" Finish Ability Ability ");

            FinishAbility();

            OnFinishedAbility?.Invoke(this);


            Log(" Exit Ability ");

            ExitAbility();

            OnEndedAbility?.Invoke(this);
        }
        public virtual void OnCancelAbility()
        {
            if (!Activate)
                return;

            _isActivate = false;


            Log(" Cancel Ability ");

            CancelAbility();

            OnCanceledAbility?.Invoke(this);


            Log(" Exit Ability ");

            ExitAbility();

            OnEndedAbility?.Invoke(this);
        }

        public void OnUpdateAbility(float deltaTime)
        {
            if (!Activate)
                return;

            UpdateAbility(deltaTime);
        }
        public void OnFixedUpdateAbility(float deltaTime)
        {
            if (!Activate)
                return;

            FixedUpdateAbility(deltaTime);
        }

        [Conditional("UNITY_EDITOR")]
        protected void Log(string log)
        {
            if (Ability.UseDebugMode)
                UnityEngine.Debug.Log(Ability.AbilityName + " : " + log, Owner);
        }

        [Conditional("UNITY_EDITOR")]
        public virtual void OnDrawGizmos() 
        {
            if (!Activate)
                return;
        }


        protected abstract void EnterAbility();
        protected abstract void ExitAbility();
        protected abstract void FinishAbility();
        protected abstract void CancelAbility();
        protected abstract void ReleasedAbility();
        protected abstract void ReTriggerAbility();
        protected virtual void UpdateAbility(float deltaTime) { }
        protected virtual void FixedUpdateAbility(float deltaTime) { }

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
