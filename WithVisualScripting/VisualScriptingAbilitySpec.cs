#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    public class VisualScriptingAbilitySpec : AbilitySpecWithMono
    {
        private bool _IsCommitActivate = false;
        public bool IsCommitActivate => _IsCommitActivate;

        public override bool CanActiveAbility()
        {
            if (!base.CanActiveAbility())
                return false;

            _IsCommitActivate = false;

            EventBus.Trigger(AbilitySystemVisualScriptingEvent.CAN_ACTIVE_ABILITY, gameObject);

            if (!IsCommitActivate)
                return false;

            return true;
        }

        public void CommitAbility()
        {
            _IsCommitActivate = true;
        }

        protected override void OnGrantAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.GRANT_ABILITY, gameObject);
        }
        protected override void OnRemoveAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.REMOVE_ABILITY, gameObject);
        }
        public override void OnOverride(int level) 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.OVERRIDE_ABILITY, gameObject, level);
        }
        protected override void EnterAbility()
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.ENTER_ABILITY, gameObject);
        }
        protected override void ExitAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.EXIT_ABILITY, gameObject);
        }
        protected override void OnFinishAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.FINISH_ABILITY, gameObject);
        }
        protected override void OnCancelAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.CANCEL_ABILITY, gameObject);
        }
        protected override void OnReleaseAbility()
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.RELEASE_ABILITY, gameObject);
        }
        protected override void OnReTriggerAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.RETRIGGER_ABILITY, gameObject);
        }
        protected override void OnChangeLevel(int prevLevel) 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.CHANGE_ABILITY_LEVEL, gameObject, prevLevel);
        }
    }
}

#endif