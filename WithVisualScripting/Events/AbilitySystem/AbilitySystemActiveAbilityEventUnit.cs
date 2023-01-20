
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("OnActivatedAbility")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySystem")]
    public class AbilitySystemActiveAbilityEventUnit : AbilitySystemCustomEventUnit
    {
        protected override string EventName => AbilitySystemVisualScriptingEvent.ABILITYSYSTEM_ACTIVE_ABILITY;
    }
}

#endif