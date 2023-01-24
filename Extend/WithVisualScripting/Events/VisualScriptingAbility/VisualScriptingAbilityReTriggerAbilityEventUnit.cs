#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On ReTrigger Ability")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityReTriggerAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string hookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_RETRIGGER_ABILITY;
    }
}

#endif