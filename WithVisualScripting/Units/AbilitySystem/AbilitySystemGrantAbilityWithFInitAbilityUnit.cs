#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("GrantAbility")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemGrantAbilityWithFInitAbilityUnit : AbilitySystemUnit
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

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("IsGrant")]
        [PortLabelHidden]
        public ValueOutput IsGrant { get; private set; }

        private bool _IsGranted;

        protected override void Definition()
        {
            base.Definition();

            Enter = ControlInput(nameof(Enter), GrantAbility);
            Exit = ControlOutput(nameof(Exit));

            Ability = ValueInput<FInitAbility>(nameof(Ability), default);

            IsGrant = ValueOutput<bool>(nameof(IsGrant), (flow) => { return _IsGranted; });
        }

        private ControlOutput GrantAbility(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);
            var ability = flow.GetValue<FInitAbility>(Ability);

            _IsGranted = abilitySystemComponent.TryGrantAbility(ability.Ability, ability.Level);

            return Exit;
        }
    }
}

#endif