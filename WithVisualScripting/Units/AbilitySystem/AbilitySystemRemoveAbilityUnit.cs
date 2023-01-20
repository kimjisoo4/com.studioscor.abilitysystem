#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("RemoveAbility")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemRemoveAbilityUnit : AbilitySystemUnit
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
        [PortLabel("IsRemove")]
        [PortLabelHidden]
        public ValueOutput IsRemove { get; private set; }

        private bool _IsRemove;

        protected override void Definition()
        {
            base.Definition();

            Enter = ControlInput(nameof(Enter), RemoveAbility);
            Exit = ControlOutput(nameof(Exit));

            Ability = ValueInput<Ability>(nameof(Ability), null);

            IsRemove = ValueOutput<bool>(nameof(IsRemove), (flow) => { return _IsRemove; });
        }

        private ControlOutput RemoveAbility(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);
            var ability = flow.GetValue<Ability>(Ability);

            _IsRemove = abilitySystemComponent.TryRemoveAbility(ability);

            return Exit;
        }
    }
}

#endif