#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Try Activate Spec")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecTryActivateUnit : AbilitySpecFlowUnit
    {
        [DoNotSerialize]
        [PortLabel("isActivate")]
        public ValueOutput IsActivate { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            IsActivate = ValueOutput<bool>(nameof(IsActivate));
        }
        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySpec = flow.GetValue<IAbilitySpec>(Target);

            flow.SetValue(IsActivate, abilitySpec.TryActiveAbility());

            return Exit;
        }
    }
}

#endif