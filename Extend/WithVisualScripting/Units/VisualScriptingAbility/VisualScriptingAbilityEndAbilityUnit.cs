#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{

    [UnitTitle("End Ability")]
    [UnitSubtitle("VisualScriptingAbility Unit")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class VisualScriptingAbilityEndAbilityUnit : Unit
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
            InputTrigger = ControlInput(nameof(InputTrigger), TriggerCummit);

            Target = ValueInput<AbilitySpecWithVisualScripting>(nameof(Target), null).NullMeansSelf();

            Requirement(Target, InputTrigger);
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