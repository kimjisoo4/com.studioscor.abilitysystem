#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("IsActivateAbility")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemIsActivateAbilityUnit : AbilitySystemUnit
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
        public ValueOutput IsActivate { get; private set; }


        protected override void Definition()
        {
            base.Definition();

            Ability = ValueInput<Ability>(nameof(Ability), null);

            IsActivate = ValueOutput<bool>(nameof(IsActivate), IsActivateAbility);
        }

        private bool IsActivateAbility(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);
            var ability = flow.GetValue<Ability>(Ability);

            return abilitySystemComponent.IsActivateAbility(ability);
        }
    }
}

#endif