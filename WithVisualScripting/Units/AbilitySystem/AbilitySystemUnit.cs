#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    public abstract class AbilitySystemUnit : Unit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput AbilitySystemComponent { get; private set; }

        protected override void Definition()
        {
            AbilitySystemComponent = ValueInput<AbilitySystemComponent>(nameof(AbilitySystemComponent), null).NullMeansSelf();
        }
    }
}

#endif