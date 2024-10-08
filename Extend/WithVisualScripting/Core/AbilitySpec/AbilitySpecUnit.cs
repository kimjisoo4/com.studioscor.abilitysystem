#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.AbilitySystem.VisualScripting
{

    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public abstract class AbilitySpecUnit : Unit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput Target { get; private set; }

        protected override void Definition()
        {
            Target = ValueInput<GameObject>(nameof(Target), null).NullMeansSelf();
        }
    }
}

#endif