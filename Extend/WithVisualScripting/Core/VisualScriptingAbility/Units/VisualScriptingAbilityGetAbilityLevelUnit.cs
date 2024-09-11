#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Get Ability Level")]
    [UnitSubtitle("VisualScriptingAbility Unit")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]

    public class VisualScriptingAbilityGetAbilityLevelUnit : Unit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput AbilitySpec { get; private set; }

        [DoNotSerialize]
        [PortLabel("Level")]
        [PortLabelHidden]
        public ValueOutput Level { get; private set; }


        protected override void Definition()
        {
            AbilitySpec = ValueInput<VisualScriptingAbilitySpec>(nameof(AbilitySpec), null).NullMeansSelf();

            Level = ValueOutput<int>(nameof(Level), GetAbility);

            Requirement(AbilitySpec, Level);
        }

        private int GetAbility(Flow flow)
        {
            var spec = flow.GetValue<VisualScriptingAbilitySpec>(AbilitySpec);

            return spec.Level;
        }
    }
}

#endif