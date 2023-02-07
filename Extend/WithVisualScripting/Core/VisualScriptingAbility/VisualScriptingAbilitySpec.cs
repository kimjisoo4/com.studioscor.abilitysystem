#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [AddComponentMenu("StudioScor/AbilitySystem/VisualScripting AbilitySpec Component", order: 10)]
    public class VisualScriptingAbilitySpec : GameObjectAbilitySpec
    {
        private OnChangedLevelValue _OnChangedLevelValue;

        private bool _IsCommitActivate = false;
        public bool IsCommitActivate => _IsCommitActivate;

        private bool _IsCommitReTrigger = false;

        public bool IsCommitReTrigger => _IsCommitReTrigger;

        public override bool CanActiveAbility()
        {
            if (!base.CanActiveAbility())
                return false;

            _IsCommitActivate = false;

            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CAN_ACTIVE_ABILITY, this));

            if (!IsCommitActivate)
                return false;

            return true;
        }

        public override bool CanReTriggerAbility()
        {
            if (!base.CanReTriggerAbility())
                return false;

            _IsCommitReTrigger = false;

            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CAN_RETRIGGER_ABILITY, this));

            if (!IsCommitReTrigger)
                return false;

            return true;
        }
        public void CommitAbility()
        {
            _IsCommitActivate = true;
        }
        public void CommitReTriggerAbility()
        {
            _IsCommitReTrigger = true;
        }

        protected override void OnGrantAbility() 
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_GRANT_ABILITY, this));
        }
        protected override void OnRemoveAbility() 
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_REMOVE_ABILITY, this));
        }
        public override void OnOverride(int level) 
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_OVERRIDE_ABILITY, this), level);
        }
        protected override void EnterAbility()
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ENTER_ABILITY, this));
        }
        protected override void ExitAbility() 
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_EXIT_ABILITY, this));
        }
        protected override void OnFinishAbility() 
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_FINISH_ABILITY, this));
        }

        public override void CancelAbilityFromSource(object source)
        {
            if (IsPlaying)
                EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCEL_ABILITY_FROM_SOURCE, this), source);
        }
        protected override void OnCancelAbility() 
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCEL_ABILITY, this));
        }
        protected override void OnReleaseAbility()
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_RELEASE_ABILITY, this));
        }
        protected override void OnReTriggerAbility() 
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_RETRIGGER_ABILITY, this));
        }
        protected override void OnChangeLevel(int prevLevel) 
        {
            _OnChangedLevelValue.CurrentLevel = Level;
            _OnChangedLevelValue.PrevLevel = prevLevel;

            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CHANGE_ABILITY_LEVEL, this), _OnChangedLevelValue);
        }
    }
}

#endif