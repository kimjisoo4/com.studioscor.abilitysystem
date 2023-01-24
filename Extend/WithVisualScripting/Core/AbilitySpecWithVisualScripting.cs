#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    public class AbilitySpecWithVisualScripting : AbilitySpecWithMono<VisualScriptingAbility>
    {
        private bool _IsCommitActivate = false;
        public bool IsCommitActivate => _IsCommitActivate;

        public override bool CanActiveAbility()
        {
            if (!base.CanActiveAbility())
                return false;

            _IsCommitActivate = false;

            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CAN_ACTIVE_ABILITY, gameObject);

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
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_GRANT_ABILITY, gameObject);
        }
        protected override void OnRemoveAbility() 
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_REMOVE_ABILITY, gameObject);
        }
        public override void OnOverride(int level) 
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_OVERRIDE_ABILITY, gameObject, level);
        }
        protected override void EnterAbility()
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ENTER_ABILITY, gameObject);
        }
        protected override void ExitAbility() 
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_EXIT_ABILITY, gameObject);
        }
        protected override void OnFinishAbility() 
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_FINISH_ABILITY, gameObject);
        }
        protected override void OnCancelAbility() 
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCEL_ABILITY, gameObject);
        }
        protected override void OnReleaseAbility()
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_RELEASE_ABILITY, gameObject);
        }
        protected override void OnReTriggerAbility() 
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_RETRIGGER_ABILITY, gameObject);
        }
        protected override void OnChangeLevel(int prevLevel) 
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CHANGE_ABILITY_LEVEL, gameObject, prevLevel);
        }
    }
}

#endif