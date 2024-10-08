#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Get AbilitySystem")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecGetAbilitySystemUnit : AbilitySpecUnit
    {
        [DoNotSerialize]
        [PortLabel("AbilitySystem")]
        [PortLabelHidden]
        public ValueOutput AbilitySystemComponent { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            AbilitySystemComponent = ValueOutput<GameObject>(nameof(AbilitySystemComponent), GetAbilitySystemComponent);

            Requirement(Target, AbilitySystemComponent);
        }

        private GameObject GetAbilitySystemComponent(Flow flow)
        {
            var spec = flow.GetValue<IAbilitySpec>(Target);

            return spec.AbilitySystem.gameObject;
        }
    }
}

#endif