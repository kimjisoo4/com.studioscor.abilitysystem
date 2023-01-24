#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Cancel Ability")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityCancelAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string hookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCEL_ABILITY;
    }
}

#endif