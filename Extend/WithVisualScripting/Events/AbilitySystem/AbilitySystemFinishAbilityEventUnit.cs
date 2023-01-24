
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Finished Ability")]
    [UnitSubtitle("AbilitySystem Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySystem")]
    public class AbilitySystemFinishAbilityEventUnit : AbilitySystemCustomEventUnit
    {
        protected override string hookName => AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_FINISH_ABILITY;
    }
}

#endif