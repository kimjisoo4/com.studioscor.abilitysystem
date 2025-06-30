using StudioScor.Utilities;
using UnityEngine;

namespace StudioScor.AbilitySystem
{
    public abstract class AbilitySpec : BaseClass, IAbilitySpec
    {
        protected readonly Ability _ability;
        protected readonly IAbilitySystem _abilitySystem;

        public Ability Ability => _ability;
        public IAbilitySystem AbilitySystem => _abilitySystem;

        protected GameObject gameObject => _abilitySystem.gameObject;
        protected Transform transform => _abilitySystem.transform;

        public int Level { get; protected set; }
        public bool IsPlaying { get; protected set; }

        public event IAbilitySpec.AbilityEventHandler OnActivatedAbility;
        public event IAbilitySpec.AbilityEventHandler OnReleasedAbility;
        public event IAbilitySpec.AbilityEventHandler OnFinishedAbility;
        public event IAbilitySpec.AbilityEventHandler OnCanceledAbility;
        public event IAbilitySpec.AbilityEventHandler OnEndedAbility;
        public event IAbilitySpec.AbilityLevelEventHandler OnChangedAbilityLevel;

#if UNITY_EDITOR
        public override bool UseDebug => _ability.UseDebug;
        public override Object Context => _ability;
#endif
        public AbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level)
        {
            this._ability = ability;
            this._abilitySystem = abilitySystem;
            this.Level = level;
        }


        public void GrantAbility()
        {
            Log($"{nameof(GrantAbility)}");

            OnGrantAbility();
        }
        public void RemoveAbility()
        {
            Log($"{nameof(RemoveAbility)}");

            CancelAbility();

            OnRemoveAbility();
        }

        public bool TryActiveAbility()
        {
            if (TryReTriggerAbility())
                return true;

            if (CanActiveAbility())
            {
                ForceActiveAbility();

                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReleaseAbility()
        {
            if (!IsPlaying)
                return;

            RaiseOnReleasedAbility();

            OnReleaseAbility();
        }
        public bool TryReTriggerAbility()
        {
            if (CanReTriggerAbility())
            {
                ForceReTriggerAbility();

                return true;
            }

            return false;
        }

        

        public void ForceActiveAbility()
        {
            IsPlaying = true;

            RaiseOnActivateAbility();

            EnterAbility();
        }

        public void ForceReTriggerAbility()
        {
            Log($"{nameof(OnReTriggerAbility)}");

            OnReTriggerAbility();
        }


        public bool TryFinishAbility()
        {
            if (CanFinishAbility())
            {
                ForceFinishAbility();

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool CanFinishAbility()
        {
            return IsPlaying;
        }

        public void ForceFinishAbility()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;


            OnFinishAbility();

            RaiseOnFinishedAbility();


            ExitAbility();

            RaiseOnEndedAbility();
        }

        public virtual void CancelAbilityFromSource(object source)
        {

        }

        public void CancelAbility()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;


            OnCancelAbility();

            Invoke_OnCanceledAbility();


            ExitAbility();

            RaiseOnEndedAbility();
        }

        public void SetAbilityLevel(int newLevel)
        {
            if (Level == newLevel)
                return;

            int prevLevel = Level;

            Level = newLevel;

            OnChangeAbilityLevel(prevLevel);

            Invoke_OnChangedAbilityLevel(prevLevel);
        }


        protected virtual void OnGrantAbility() { }
        protected virtual void OnRemoveAbility() { }



        protected abstract void EnterAbility();
        protected virtual void ExitAbility() { }
        protected virtual void OnFinishAbility() { }
        protected virtual void OnCancelAbility() { }
        protected virtual void OnReleaseAbility() { }
        protected virtual void OnReTriggerAbility() { }
        protected virtual void OnChangeAbilityLevel(int prevLevel) { }

        public virtual bool CanReTriggerAbility()
        {
            return IsPlaying;
        }

        public virtual bool CanActiveAbility() 
        {
            return !IsPlaying;
        }

        #region Callback
        protected virtual void RaiseOnActivateAbility()
        {
            Log(nameof(OnActivatedAbility));

            OnActivatedAbility?.Invoke(this);
        }
        protected virtual void RaiseOnReleasedAbility()
        {
            Log(nameof(OnReleasedAbility));

            OnReleasedAbility?.Invoke(this);
        }
        protected virtual void RaiseOnFinishedAbility()
        {
            Log(nameof(OnFinishedAbility));

            OnFinishedAbility?.Invoke(this);
        }
        protected virtual void RaiseOnEndedAbility()
        {
            Log(nameof(OnEndedAbility));

            OnEndedAbility?.Invoke(this);
        }
        protected virtual void Invoke_OnCanceledAbility()
        {
            Log(nameof(OnCanceledAbility));

            OnCanceledAbility?.Invoke(this);
        }
        protected virtual void Invoke_OnChangedAbilityLevel(int prevLevel)
        {
            Log("Level Change - Current Level : " + Level + " Prev Level : " + prevLevel);

            OnChangedAbilityLevel?.Invoke(this, Level, prevLevel);
        }

        #endregion
    }
}
