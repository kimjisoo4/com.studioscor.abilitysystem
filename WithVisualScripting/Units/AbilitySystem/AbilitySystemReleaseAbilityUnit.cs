#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("ReleaseAbility")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemReleaseAbilityUnit : AbilitySystemUnit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput Enter;

        [DoNotSerialize]
        [PortLabel("Exit")]
        [PortLabelHidden]
        public ControlOutput Exit;

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Enter = ControlInput(nameof(Enter), CancelAbility);
            Exit = ControlOutput(nameof(Exit));

            Ability = ValueInput<Ability>(nameof(Ability), null);
        }

        private ControlOutput CancelAbility(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);
            var ability = flow.GetValue<Ability>(Ability);

            abilitySystemComponent.ReleasedAbility(ability);

            return Exit;
        }
    }
}

#endif