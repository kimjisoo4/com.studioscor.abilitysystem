#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Changed Ability Level")]
    [UnitSubtitle("AbilitySpec Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecChangedAbilityLevelEventUnit : AbilitySpecLevelChangedEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CHANGED_ABILITY_LEVEL;
    }
}

#endif