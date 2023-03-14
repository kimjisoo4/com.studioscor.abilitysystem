#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{

    [UnitTitle("Get AbilitySystem")]
    [UnitSubtitle("VisualScriptingAbility Unit")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class VisualScriptingAbilityGetAbilitySystemUnit : Unit
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
            AbilitySpec = ValueInput<VisualScriptingAbilitySpec>(nameof(AbilitySpec), null).NullMeansSelf();

            AbilitySystemComponent = ValueOutput<IAbilitySystem>(nameof(AbilitySystemComponent), GetAbilitySystemComponent);

            Requirement(AbilitySpec, AbilitySystemComponent);
        }

        private IAbilitySystem GetAbilitySystemComponent(Flow flow)
        {
            var spec = flow.GetValue<VisualScriptingAbilitySpec>(AbilitySpec);

            return spec.AbilitySystem;
        }
    }
}

#endif