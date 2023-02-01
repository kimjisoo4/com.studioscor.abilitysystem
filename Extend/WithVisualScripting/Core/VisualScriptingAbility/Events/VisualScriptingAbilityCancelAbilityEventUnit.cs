#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Cancel Ability")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityCancelAbilityEventUnit : VisualScriptingAbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCEL_ABILITY;
    }
}

#endif