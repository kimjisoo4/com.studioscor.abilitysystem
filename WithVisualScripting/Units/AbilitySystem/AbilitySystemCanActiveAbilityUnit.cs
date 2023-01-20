#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("CanActiveAbility")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemCanActiveAbilityUnit : AbilitySystemUnit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("CanActivate")]
        [PortLabelHidden]
        public ValueOutput CanActive { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Ability = ValueInput<Ability>(nameof(Ability), null);

            CanActive = ValueOutput<bool>(nameof(CanActive), CanActiveAbility);
        }

        private bool CanActiveAbility(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);
            var ability = flow.GetValue<Ability>(Ability);

            return abilitySystemComponent.CanActivateAbility(ability);
        }
    }
}

#endif