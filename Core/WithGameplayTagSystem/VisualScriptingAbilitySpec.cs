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

        protected override void GrantAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.GRANT_ABILITY, gameObject);
        }
        protected override void LostAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.LOST_ABILITY, gameObject);
        }
        public override void OnOverrideAbility(int level) 
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
        protected override void FinishAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.FINISH_ABILITY, gameObject);
        }
        protected override void CancelAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.CANCEL_ABILITY, gameObject);
        }
        protected override void ReleaseAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.RELEASE_ABILITY, gameObject);
        }
        protected override void ReTriggerAbility() 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.RETRIGGER_ABILITY, gameObject);
        }
        protected override void ChangeAbilityLevel(int prevLevel) 
        {
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.CHANGE_ABILITY_LEVEL, gameObject, prevLevel);
        }
    }
}

#endif