#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{

    [UnitTitle("CancelAbility")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecCancelAbilityUnit : Unit
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

            Target = ValueInput<AbilitySpecWithVisualScripting>(nameof(Target), null).NullMeansSelf();
        }

        private ControlOutput TriggerCummit(Flow flow)
        {
            var abilitySpec = flow.GetValue<AbilitySpecWithVisualScripting>(Target);

            abilitySpec.EndAbility();

            return null;
        }
    }
}

#endif