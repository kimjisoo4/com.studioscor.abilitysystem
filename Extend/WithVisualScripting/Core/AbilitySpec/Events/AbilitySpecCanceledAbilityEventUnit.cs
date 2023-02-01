#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Canceled Ability")]
    [UnitSubtitle("AbilitySpec Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecCanceledAbilityEventUnit : AbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCELED_ABILITY;

    }
}

#endif