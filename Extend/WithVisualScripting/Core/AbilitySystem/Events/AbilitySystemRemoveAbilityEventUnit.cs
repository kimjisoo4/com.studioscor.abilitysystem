
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Removed Ability")]
    [UnitSubtitle("AbilitySystem Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySystem")]
    public class AbilitySystemRemoveAbilityEventUnit : AbilitySystemEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_REMOVE_ABILITY;
    }
}

#endif