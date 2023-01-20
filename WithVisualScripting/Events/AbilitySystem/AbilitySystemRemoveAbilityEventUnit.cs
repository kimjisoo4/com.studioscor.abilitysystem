
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("OnRemovedAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySystem")]
    public class AbilitySystemRemoveAbilityEventUnit : AbilitySystemCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.ABILITYSYSTEM_REMOVE_ABILITY;
    }
}

#endif