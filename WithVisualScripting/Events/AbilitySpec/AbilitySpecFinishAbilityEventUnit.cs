#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("OnFinishAbilityLevel")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecFinishAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.ABILITYSPEC_FINISH_ABILITY;
    }
}

#endif