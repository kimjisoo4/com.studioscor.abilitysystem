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

        public event AbilityEventHandler OnActivatedAbility;
        public event AbilityEventHandler OnReleasedAbility;
        public event AbilityEventHandler OnFinishedAbility;
        public event AbilityEventHandler OnCanceledAbility;
        public event AbilityEventHandler OnEndedAbility;
        public event AbilityLevelEventHandler OnChangedAbilityLevel;

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
            Log("Grant Ability");

            OnGrantAbility();
        }
        public void RemoveAbility()
        {
            Log("Remove Ability ");

            ForceFinishAbility();

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

            Log(" On Release Ability ");

            Invoke_OnReleasedAbility();

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

        

        public virtual void ForceActiveAbility()
        {
            Log(" Active Ability ");

            IsPlaying = true;

            Invoke_OnActivateAbility();

            EnterAbility();
        }

        public void ForceReTriggerAbility()
        {
            Log(" ReTrigger Ability ");

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

        public virtual void ForceFinishAbility()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;


            OnFinishAbility();

            Invoke_OnFinishedAbility();


            ExitAbility();

            Invoke_OnEndedAbility();
        }

        public virtual void CancelAbilityFromSource(object source)
        {

        }

        public virtual void CancelAbility()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;


            OnCancelAbility();

            Invoke_OnCanceledAbility();


            ExitAbility();

            Invoke_OnEndedAbility();
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



        public virtual void OnOverride(int level) { }
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
        protected virtual void Invoke_OnActivateAbility()
        {
            Log(nameof(OnActivatedAbility));

            OnActivatedAbility?.Invoke(this);
        }
        protected virtual void Invoke_OnReleasedAbility()
        {
            Log(nameof(OnReleasedAbility));

            OnReleasedAbility?.Invoke(this);
        }
        protected virtual void Invoke_OnFinishedAbility()
        {
            Log(nameof(OnFinishedAbility));

            OnFinishedAbility?.Invoke(this);
        }
        protected virtual void Invoke_OnEndedAbility()
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
