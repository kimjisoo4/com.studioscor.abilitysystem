#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("CanActiveAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecCanActiveAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.CAN_ACTIVE_ABILITY;
    }
    [UnitTitle("OnCancelAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecCancelAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.CANCEL_ABILITY;
    }
    [UnitTitle("OnReTriggerAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecReTriggerAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.RETRIGGER_ABILITY;
    }
    [UnitTitle("OnChangeAbilityLevel")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecChangeAbilityLevelEventUnit : AbilitySpecLevelCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.CHANGE_ABILITY_LEVEL;
    }
    [UnitTitle("OnEnterAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecOnEnterAbilityAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.ENTER_ABILITY;
    }
    [UnitTitle("OnExitAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecExitAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.EXIT_ABILITY;
    }
    [UnitTitle("OnFinishAbilityLevel")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecFinishAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.FINISH_ABILITY;
    }
    [UnitTitle("OnGrantAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecGrantAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.GRANT_ABILITY;
    }
    [UnitTitle("OnLostAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecLostAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.LOST_ABILITY;
    }
    [UnitTitle("OnOverrideAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecOverrideAbilityEventUnit : AbilitySpecLevelCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.OVERRIDE_ABILITY;
    }
    [UnitTitle("OnReleaseAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecReleaseAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.RELEASE_ABILITY;
    }
}

#endif