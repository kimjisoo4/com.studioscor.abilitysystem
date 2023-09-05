using StudioScor.Utilities;
using UnityEngine;

namespace StudioScor.AbilitySystem
{
    public abstract class AbilitySpec : BaseClass, IAbilitySpec, IAbilitySpecEvent
    {
        protected readonly Ability ability;
        protected readonly IAbilitySystem abilitySystem;

        protected Transform transform => abilitySystem.transform;

        public Ability Ability => ability;
        public IAbilitySystem AbilitySystem => abilitySystem;
        public int Level { get; protected set; }
        public bool IsPlaying { get; protected set; }

        public event AbilityEventHandler OnActivatedAbility;
        public event AbilityEventHandler OnReleasedAbility;
        public event AbilityEventHandler OnEndedAbility;
        public event AbilityEventHandler OnFinishedAbility;
        public event AbilityEventHandler OnCanceledAbility;
        public event AbilityLevelEventHandler OnChangedAbilityLevel;

#if UNITY_EDITOR
        public override bool UseDebug => ability.UseDebug;
        public override Object Context => ability;
#endif
        public AbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level)
        {
            this.ability = ability;
            this.abilitySystem = abilitySystem;
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

            ForceEndAbility();

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

            CallBack_OnReleasedAbility();

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

            CallBack_OnActivateAbility();

            EnterAbility();
        }

        public void ForceReTriggerAbility()
        {
            Log(" ReTrigger Ability ");

            OnReTriggerAbility();
        }


        public bool TryEndAbility()
        {
            if (CanEndAbility())
            {
                ForceEndAbility();

                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool CanEndAbility()
        {
            return IsPlaying;
        }

        public virtual void ForceEndAbility()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;


            OnFinishAbility();

            CallBack_OnFinishedAbility();


            ExitAbility();

            CallBack_OnEndedAbility();
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

            CallBack_OnCanceledAbility();


            ExitAbility();

            CallBack_OnEndedAbility();
        }

        public void SetAbilityLevel(int newLevel)
        {
            if (Level == newLevel)
                return;

            int prevLevel = Level;

            Level = newLevel;

            OnChangeAbilityLevel(prevLevel);

            Callback_OnChangedAbilityLevel(prevLevel);
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
        protected virtual void CallBack_OnActivateAbility()
        {
            Log("On Activated Ability");

            OnActivatedAbility?.Invoke(this);
        }
        protected virtual void CallBack_OnReleasedAbility()
        {
            Log("On Released Ability");

            OnReleasedAbility?.Invoke(this);
        }
        protected virtual void CallBack_OnFinishedAbility()
        {
            Log("On Finished Ability");

            OnFinishedAbility?.Invoke(this);
        }
        protected virtual void CallBack_OnEndedAbility()
        {
            Log("On Ended Ability");

            OnEndedAbility?.Invoke(this);
        }
        protected virtual void CallBack_OnCanceledAbility()
        {
            Log("On Canceled Ability");

            OnCanceledAbility?.Invoke(this);
        }
        protected virtual void Callback_OnChangedAbilityLevel(int prevLevel)
        {
            Log("Level Change - Current Level : " + Level + " Prev Level : " + prevLevel);

            OnChangedAbilityLevel?.Invoke(this, Level, prevLevel);
        }

        #endregion
    }
}
