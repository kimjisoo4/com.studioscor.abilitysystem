#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("OnOverrideAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecOverrideAbilityEventUnit : AbilitySpecLevelCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.OVERRIDE_ABILITY;
    }
}

#endif