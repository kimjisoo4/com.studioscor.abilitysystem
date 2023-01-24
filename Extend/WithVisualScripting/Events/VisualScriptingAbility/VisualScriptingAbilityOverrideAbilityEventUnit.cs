#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Override Ability")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityOverrideAbilityEventUnit : AbilitySpecLevelCustomEventUnit
    {
        protected override string hookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_OVERRIDE_ABILITY;
    }
}

#endif