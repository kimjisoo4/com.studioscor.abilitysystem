#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{

    public abstract class AbilitySpecFlowUnit : AbilitySpecUnit
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

            Enter = ControlInput(nameof(Enter), EnterUnit);
            Exit = ControlOutput(nameof(Exit));

            Requirement(Target, Enter);
            Succession(Enter, Exit);
        }

        protected abstract ControlOutput EnterUnit(Flow flow);
    }
}

#endif