#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Try Cancel Ability From Source")]
    [UnitShortTitle("TryCancelAbilityFromSource")]
    [UnitSubtitle("VisualScripting Ability Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityTryCancelAbilityFromSourceEventUnit : CustomEventUnit<VisualScriptingAbilitySpec, object>
    {
        public override Type MessageListenerType => null;
        protected override string HookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCEL_ABILITY_FROM_SOURCE;

        [DoNotSerialize]
        [PortLabel("Source")]
        [PortLabelHidden]
        public ValueOutput Source;

        protected override void Definition()
        {
            base.Definition();

            Source = ValueOutput<object>(nameof(Source));
        }
        protected override void AssignArguments(Flow flow, object value)
        {
            base.AssignArguments(flow, value);

            flow.SetValue(Source, value);
        }
    }
}

#endif