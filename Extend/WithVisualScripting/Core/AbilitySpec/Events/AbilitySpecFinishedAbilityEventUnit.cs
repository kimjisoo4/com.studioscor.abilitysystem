#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Finished Ability")]
    [UnitSubtitle("AbilitySpec Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecFinishedAbilityEventUnit : AbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_FINISHED_ABILITY;

    }
}

#endif