#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    [UnitTitle("ResetAbilitySystemComponent")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemResetAbilitySystemComponentUnit : AbilitySystemUnit
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

            Enter = ControlInput(nameof(Enter), ResetAbilitySystemComponent);
            Exit = ControlOutput(nameof(Exit));
        }

        private ControlOutput ResetAbilitySystemComponent(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);

            abilitySystemComponent.ResetAbilitySystemComponent();

            return Exit;
        }
    }
}

#endif