#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Activated Ability")]
    [UnitSubtitle("AbilitySpec Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecActivatedAbilityEventUnit : AbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ACTIVATED_ABILITY;

    }
}

#endif