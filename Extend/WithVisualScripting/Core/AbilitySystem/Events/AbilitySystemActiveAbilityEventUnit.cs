
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Activated Ability")]
    [UnitSubtitle("AbilitySystem Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySystem")]
    public class AbilitySystemActiveAbilityEventUnit : AbilitySystemEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_ACTIVE_ABILITY;
    }
}

#endif