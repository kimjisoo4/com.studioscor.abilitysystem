#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Ended Ability")]
    [UnitSubtitle("AbilitySpec Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecEndedAbilityEventUnit : AbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ENDED_ABILITY;

    }
}

#endif