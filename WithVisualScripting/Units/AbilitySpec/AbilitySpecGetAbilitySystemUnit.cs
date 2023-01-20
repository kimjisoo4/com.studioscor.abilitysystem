#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{

    [UnitTitle("GetAbility")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecGetAbilityUnit : Unit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput AbilitySpec { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySystem")]
        [PortLabelHidden]
        public ValueOutput Ability { get; private set; }


        protected override void Definition()
        {
            AbilitySpec = ValueInput<AbilitySpecWithVisualScripting>(nameof(AbilitySpec), null).NullMeansSelf();

            Ability = ValueOutput<Ability>(nameof(Ability), GetAbility);

            Requirement(AbilitySpec, Ability);
        }

        private Ability GetAbility(Flow flow)
        {
            var spec = flow.GetValue<AbilitySpecWithVisualScripting>(AbilitySpec);

            return spec.Ability;
        }
    }

    [UnitTitle("GetAbilitySystem")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecGetAbilitySystemUnit : Unit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput AbilitySpec { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySystem")]
        [PortLabelHidden]
        public ValueOutput AbilitySystemComponent { get; private set; }


        protected override void Definition()
        {
            AbilitySpec = ValueInput<AbilitySpecWithVisualScripting>(nameof(AbilitySpec), null).NullMeansSelf();

            AbilitySystemComponent = ValueOutput<AbilitySystemComponent>(nameof(AbilitySystemComponent), GetAbilitySystemComponent);

            Requirement(AbilitySpec, AbilitySystemComponent);
        }

        private AbilitySystemComponent GetAbilitySystemComponent(Flow flow)
        {
            var spec = flow.GetValue<AbilitySpecWithVisualScripting>(AbilitySpec);

            return spec.AbilitySystemComponent;
        }
    }
}

#endif