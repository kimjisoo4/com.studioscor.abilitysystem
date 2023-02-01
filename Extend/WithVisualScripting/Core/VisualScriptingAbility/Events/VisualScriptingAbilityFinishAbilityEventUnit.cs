#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Finish Ability Level")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityFinishAbilityEventUnit : VisualScriptingAbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_FINISH_ABILITY;
    }
}

#endif