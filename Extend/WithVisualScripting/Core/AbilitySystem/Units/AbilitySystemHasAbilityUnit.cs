
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Has Ability")]
    [UnitSubtitle("AbilitySystem Unit")]
    public class AbilitySystemHasAbilityUnit : AbilitySystemUnit
    {
        [DoNotSerialize]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        [DoNotSerialize]
        [PortLabel("hasAbility")]
        [PortLabelHidden]
        public ValueOutput HasAbility { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Ability = ValueInput<Ability>(nameof(Ability), null);
            HasAbility = ValueOutput<bool>(nameof(HasAbility), CheckHasAbility);

            Requirement(Target, HasAbility);
            Requirement(Ability, HasAbility);
        }

        private bool CheckHasAbility(Flow flow)
        {
            var abilitySystem = flow.GetValue<IAbilitySystem>(Target);
            var ability = flow.GetValue<Ability>(Ability);

            return abilitySystem.HasAbility(ability);
        }
    }
}

#endif