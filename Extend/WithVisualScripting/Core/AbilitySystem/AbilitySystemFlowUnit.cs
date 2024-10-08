
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{

    public abstract class AbilitySystemFlowUnit : AbilitySystemUnit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit { get; private set; }


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