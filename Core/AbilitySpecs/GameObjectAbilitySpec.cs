using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.AbilitySystem
{
    public abstract class GameObjectAbilitySpec : BaseMonoBehaviour, IAbilitySpec
    {
        protected Ability _Ability;
        protected AbilitySystemComponent _AbilitySystemComponent;

        protected int _Level = 0;
        protected bool _IsPlaying = false;

        public event AbilityEventHandler OnActivatedAbility;
        public event AbilityEventHandler OnEndedAbility;
        public event AbilityEventHandler OnFinishedAbility;
        public event AbilityEventHandler OnCanceledAbility;
        public event AbilityLevelEventHandler OnChangedAbilityLevel;

        public Ability Ability => _Ability;
        public AbilitySystemComponent AbilitySystemComponent => _AbilitySystemComponent;
        public int Level => _Level;
        public bool IsPlaying => _IsPlaying;


        public virtual void Setup(Ability ability, AbilitySystemComponent abilitySystemComponent, int level = 0)
        {
            _Ability = ability;
            _AbilitySystemComponent = abilitySystemComponent;
            _Level = level;
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

            _IsPlaying = true;

            Callback_OnActivatedAbility();

            EnterAbility();
        }

        public void ForceReTriggerAbility()
        {
            Log(" ReTrigger Ability ");

            OnReTriggerAbility();
        }

        public virtual void EndAbility()
        {
            if (!IsPlaying)
                return;

            _IsPlaying = false;


            OnFinishAbility();

            Callback_OnFinishedAbility();


            ExitAbility();

            Callback_OnEndedAbility();
        }
        public virtual void ForceCancelAbility()
        {
            if (!IsPlaying)
                return;

            _IsPlaying = false;


            OnCancelAbility();

            Callback_OnCanceldAbility();


            ExitAbility();

            Callback_OnEndedAbility();
        }

        public void UpdateAbility(float deltaTime)
        {
            if (!IsPlaying)
                return;

            OnUpdateAbility(deltaTime);
        }
        public void FixedUpdateAbility(float deltaTime)
        {
            if (!IsPlaying)
                return;

            OnFixedUpdateAbility(deltaTime);
        }

        public void SetAbilityLevel(int newLevel)
        {
            if (Level == newLevel)
                return;

            int prevLevel = Level;

            _Level = newLevel;

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
        public virtual void OnUpdateAbility(float deltaTime) { }
        public virtual void OnFixedUpdateAbility(float deltaTime) { }


        public virtual bool CanReTriggerAbility()
        {
            return IsPlaying;
        }

        public virtual bool CanActiveAbility()
        {
            return true;
        }


        #region Callback
        protected virtual void Callback_OnActivatedAbility()
        {
            Log("On Activated Ability");

            OnActivatedAbility?.Invoke(this);
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
