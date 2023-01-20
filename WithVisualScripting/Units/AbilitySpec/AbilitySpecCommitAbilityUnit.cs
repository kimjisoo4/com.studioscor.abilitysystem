#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;


namespace StudioScor.AbilitySystem
{

    [UnitTitle("CommitAbility")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecCommitAbilityUnit : Unit
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

            abilitySpec.CommitAbility();

            return null;
        }
    }
}

#endif