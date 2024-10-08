
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySystem")]
    public abstract class AbilitySystemUnit : Unit
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