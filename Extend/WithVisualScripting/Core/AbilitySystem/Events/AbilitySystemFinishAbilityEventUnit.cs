
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Finished Ability")]
    [UnitSubtitle("AbilitySystem Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySystem")]
    public class AbilitySystemFinishAbilityEventUnit : AbilitySystemEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_END_ABILITY;
    }
}

#endif