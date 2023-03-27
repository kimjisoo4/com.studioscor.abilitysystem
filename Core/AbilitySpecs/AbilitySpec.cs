﻿using StudioScor.Utilities;
using UnityEngine;

namespace StudioScor.AbilitySystem
{
    public abstract class AbilitySpec : BaseClass, IAbilitySpec, IAbilitySpecEvent
    {
        protected readonly Ability _Ability;
        protected readonly IAbilitySystem _AbilitySystem;

        protected int _Level = 0;
        protected bool _IsPlaying = false;

        protected Transform transform => _AbilitySystem.transform;

        public Ability Ability => _Ability;
        public IAbilitySystem AbilitySystem => _AbilitySystem;
        public int Level => _Level;
        public bool IsPlaying => _IsPlaying;

        public event AbilityEventHandler OnActivatedAbility;
        public event AbilityEventHandler OnReleasedAbility;
        public event AbilityEventHandler OnEndedAbility;
        public event AbilityEventHandler OnFinishedAbility;
        public event AbilityEventHandler OnCanceledAbility;
        public event AbilityLevelEventHandler OnChangedAbilityLevel;

#if UNITY_EDITOR
        public override bool UseDebug => _Ability.UseDebug;
        public override Object Context => _Ability;
#endif
        public AbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level)
        {
            _Ability = ability;
            _AbilitySystem = abilitySystem;
            _Level = level;
        }


        public void GrantAbility()
        {
            Log("Grant Ability");

            OnGrantAbility();
        }
        public void RemoveAbility()
        {
            Log("Remove Ability ");

            EndAbility();

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

            Log(" On Released Ability ");

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
            Log(" On Ability ");

            _IsPlaying = true;

            CallBack_OnActivateAbility();

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

            _IsPlaying = false;


            OnCancelAbility();

            CallBack_OnCanceledAbility();


            ExitAbility();

            CallBack_OnEndedAbility();
        }

        public void UpdateAbility(float deltaTime)
        {
            OnUpdateAlways(deltaTime);

            if (!IsPlaying)
                return;

            OnUpdateAbility(deltaTime);
        }
        public virtual void OnUpdateAlways(float deltaTime) { }
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

            OnChangeAbilityLevel(prevLevel);

            Callback_OnChangedAbilityLevel(prevLevel);
        }


        protected virtual void OnGrantAbility() { }
        protected virtual void OnRemoveAbility() { }



        public void OnOverride(int level) { }
        protected abstract void EnterAbility();
        protected virtual void ExitAbility() { }
        protected virtual void OnFinishAbility() { }
        protected virtual void OnCancelAbility() { }
        protected virtual void OnReleaseAbility() { }
        protected virtual void OnReTriggerAbility() { }
        public virtual void OnUpdateAbility(float deltaTime) { }
        public virtual void OnFixedUpdateAbility(float deltaTime) { }
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
