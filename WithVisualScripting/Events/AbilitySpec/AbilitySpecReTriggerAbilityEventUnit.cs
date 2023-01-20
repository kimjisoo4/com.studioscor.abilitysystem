#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("OnReTriggerAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecReTriggerAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.ABILITYSPEC_RETRIGGER_ABILITY;
    }
}

#endif