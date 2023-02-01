#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On ReTrigger Ability")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityReTriggerAbilityEventUnit : VisualScriptingAbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_RETRIGGER_ABILITY;
    }
}

#endif