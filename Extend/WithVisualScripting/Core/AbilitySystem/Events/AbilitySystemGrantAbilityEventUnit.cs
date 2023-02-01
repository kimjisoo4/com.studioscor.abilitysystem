
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Granted Ability")]
    [UnitSubtitle("AbilitySystem Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySystem")]
    public class AbilitySystemGrantAbilityEventUnit : AbilitySystemEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_GRANT_ABILITY;
    }
}

#endif