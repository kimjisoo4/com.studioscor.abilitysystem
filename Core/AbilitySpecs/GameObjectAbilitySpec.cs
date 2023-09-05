using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.AbilitySystem
{
    public abstract class GameObjectAbilitySpec : BaseMonoBehaviour, IAbilitySpec, IAbilitySpecEvent
    {
        protected Ability ability;
        protected IAbilitySystem abilitySystem;

        public event AbilityEventHandler OnActivatedAbility;
        public event AbilityEventHandler OnReleasedAbility;
        public event AbilityEventHandler OnEndedAbility;
        public event AbilityEventHandler OnFinishedAbility;
        public event AbilityEventHandler OnCanceledAbility;
        public event AbilityLevelEventHandler OnChangedAbilityLevel;

        public Ability Ability => ability;
        public IAbilitySystem AbilitySystem => abilitySystem;

        public int Level { get; protected set; }
        public bool IsPlaying { get; protected set; }


        public virtual void Setup(Ability ability, IAbilitySystem abilitySystem, int level = 0)
        {
            this.ability = ability;
            this.abilitySystem = abilitySystem;
            this.Level = level;
        }

        public void GrantAbility()
        {
            OnGrantAbility();
        }
        public void RemoveAbility()
        {
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

            Callback_OnReleasedAbility();

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
            Log(" On Ability ");

            IsPlaying = true;

            Callback_OnActivatedAbility();

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

            Callback_OnFinishedAbility();


            ExitAbility();

            Callback_OnEndedAbility();
        }

        public virtual void CancelAbilityFromSource(object source) { }

        public virtual void CancelAbility()
        {
            if (!IsPlaying)
                return;

            IsPlaying = false;


            OnCancelAbility();

            Callback_OnCanceldAbility();


            ExitAbility();

            Callback_OnEndedAbility();
        }

        public void SetAbilityLevel(int newLevel)
        {
            if (Level == newLevel)
                return;

            int prevLevel = Level;

            Level = newLevel;

            OnChangeLevel(prevLevel);

            Callback_OnChangedAbilityLevel(prevLevel);
        }


        protected virtual void OnGrantAbility() { }
        protected virtual void OnRemoveAbility() { }

        public virtual void OnOverride(int level) { }
        protected virtual void OnChangeLevel(int prevLevel) { }

        protected abstract void EnterAbility();
        protected virtual void ExitAbility() { }
        protected virtual void OnFinishAbility() { }
        protected virtual void OnCancelAbility() { }
        protected virtual void OnReleaseAbility() { }
        protected virtual void OnReTriggerAbility() { }


        public virtual bool CanReTriggerAbility()
        {
            return IsPlaying;
        }

        public virtual bool CanActiveAbility()
        {
            return !IsPlaying;
        }


        #region Callback
        protected virtual void Callback_OnActivatedAbility()
        {
            Log("On Activated Ability");

            OnActivatedAbility?.Invoke(this);
        }

        protected virtual void Callback_OnReleasedAbility()
        {
            Log("On Released Ability");

            OnReleasedAbility?.Invoke(this);
        }
        protected virtual void Callback_OnFinishedAbility()
        {
            Log("On Finished Ability");

            OnFinishedAbility?.Invoke(this);
        }
        protected virtual void Callback_OnCanceldAbility()
        {
            Log("On Canceled Ability");

            OnCanceledAbility?.Invoke(this);
        }
        protected virtual void Callback_OnEndedAbility()
        {
            Log("On Ended Ability");

            OnEndedAbility?.Invoke(this);
        }
        protected virtual void Callback_OnChangedAbilityLevel(int prevLevel)
        {
            Log("Level Change - Current Level : " + Level + " Prev Level : " + prevLevel);

            OnChangedAbilityLevel?.Invoke(this, Level, prevLevel);
        }


        #endregion
    }
}
