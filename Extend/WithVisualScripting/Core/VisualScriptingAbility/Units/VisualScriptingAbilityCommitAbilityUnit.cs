#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("CommitAbility")]
    [UnitSubtitle("VisualScripting Ability Unit")]
    [UnitCategory("StudioScor\\AbilitySystem\\VisualScriptingAbility")]
    public class VisualScriptingAbilityCommitAbilityUnit : Unit
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

            Target = ValueInput<VisualScriptingAbilitySpec>(nameof(Target), null).NullMeansSelf();

            Requirement(Target, InputTrigger);
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