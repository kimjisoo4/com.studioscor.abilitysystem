#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Get Ability")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecGetAbilityUnit : AbilitySpecUnit
    {
        [DoNotSerialize]
        [PortLabel("AbilitySystem")]
        [PortLabelHidden]
        public ValueOutput Ability { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Ability = ValueOutput<Ability>(nameof(Ability), GetAbility);

            Requirement(Target, Ability);
        }

        private Ability GetAbility(Flow flow)
        {
            var spec = flow.GetValue<IAbilitySpec>(Target);

            return spec.Ability;
        }
    }
}

#endif