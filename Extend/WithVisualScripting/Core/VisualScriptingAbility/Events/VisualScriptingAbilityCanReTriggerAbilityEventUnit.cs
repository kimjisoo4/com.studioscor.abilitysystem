#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Can ReTrigger Ability")]
    [UnitShortTitle("CanReTriggerAbility")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityCanReTriggerAbilityEventUnit : VisualScriptingAbilitySpecEventUnit
    {
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CAN_RETRIGGER_ABILITY;
    }
}

#endif