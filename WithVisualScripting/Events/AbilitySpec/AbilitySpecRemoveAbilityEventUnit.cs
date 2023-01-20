#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("OnRemoveAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecRemoveAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.ABILITYSPEC_REMOVE_ABILITY;
    }
}

#endif