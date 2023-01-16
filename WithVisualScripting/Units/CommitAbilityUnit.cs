#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;


namespace StudioScor.AbilitySystem
{

    [UnitTitle("CancelAbility")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class CancelAbilityUnit : Unit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput InputTrigger;

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput Target { get; private set; }
        protected override void Definition()
        {
            InputTrigger = ControlInput("InputTrigger", TriggerCummit);

            Target = ValueInput<VisualScriptingAbilitySpec>(nameof(Target), null).NullMeansSelf();
        }

        private ControlOutput TriggerCummit(Flow flow)
        {
            var abilitySpec = flow.GetValue<VisualScriptingAbilitySpec>(Target);

            abilitySpec.EndAbility();

            return null;
        }
    }

    [UnitTitle("EndAbility")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class EndAbilityUnit : Unit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput InputTrigger;

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput Target { get; private set; }

        protected override void Definition()
        {
            InputTrigger = ControlInput("InputTrigger", TriggerCummit);

            Target = ValueInput<VisualScriptingAbilitySpec>(nameof(Target), null).NullMeansSelf();
        }

        private ControlOutput TriggerCummit(Flow flow)
        {
            var abilitySpec = flow.GetValue<VisualScriptingAbilitySpec>(Target);

            abilitySpec.EndAbility();

            return null;
        }
    }

    [UnitTitle("CommitAbility")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class CommitAbilityUnit : Unit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput InputTrigger;

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput Target { get; private set; }

        protected override void Definition()
        {
            InputTrigger = ControlInput("InputTrigger", TriggerCummit);

            Target = ValueInput<VisualScriptingAbilitySpec>(nameof(Target), null).NullMeansSelf();
        }

        private ControlOutput TriggerCummit(Flow flow)
        {
            var abilitySpec = flow.GetValue<VisualScriptingAbilitySpec>(Target);

            abilitySpec.CommitAbility();

            return null;
        }
    }
}

#endif