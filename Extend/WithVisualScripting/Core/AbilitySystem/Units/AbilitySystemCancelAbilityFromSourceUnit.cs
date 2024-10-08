
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Cancel Ability From Source")]
    [UnitSubtitle("AbilitySystem Unit")]
    public class AbilitySystemCancelAbilityFromSourceUnit : AbilitySystemFlowUnit
    {
        [DoNotSerialize]
        [PortLabel("Source")]
        [PortLabelHidden]
        public ValueInput Source { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Source = ValueInput<object>(nameof(Source));

            Requirement(Source, Enter);
        }

        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySystem = flow.GetValue<IAbilitySystem>(Target);
            var source = flow.GetValue<object>(Source);

            abilitySystem.CancelAbilityFromSource(source);

            return Exit;
        }
    }
}

#endif