#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Change Ability Level")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityChangeAbilityLevelEventUnit : VisualScriptingAbilitySpecLevelChangedEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CHANGE_ABILITY_LEVEL;
    }
}

#endif