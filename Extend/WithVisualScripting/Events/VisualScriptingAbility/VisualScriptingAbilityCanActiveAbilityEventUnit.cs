#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Can Active Ability")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityCanActiveAbilityEventUnit : AbilitySpecCustomEventUnit
    {
        protected override string hookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CAN_ACTIVE_ABILITY;
    }
}

#endif