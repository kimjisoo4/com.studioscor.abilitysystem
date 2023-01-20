#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("RemoveAllAbility")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemRemoveAllAbilityUnit : AbilitySystemUnit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput Enter;

        [DoNotSerialize]
        [PortLabel("Exit")]
        [PortLabelHidden]
        public ControlOutput Exit;

        protected override void Definition()
        {
            base.Definition();

            Enter = ControlInput(nameof(Enter), RemoveAllAbility);
            Exit = ControlOutput(nameof(Exit));
        }

        private ControlOutput RemoveAllAbility(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);

            abilitySystemComponent.RemoveAllAbility();

            return Exit;
        }
    }
}

#endif