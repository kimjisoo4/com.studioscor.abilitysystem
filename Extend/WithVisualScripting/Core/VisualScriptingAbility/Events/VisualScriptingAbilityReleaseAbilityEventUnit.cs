#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Release Ability")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityReleaseAbilityEventUnit : VisualScriptingAbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_RELEASE_ABILITY;
    }
}

#endif