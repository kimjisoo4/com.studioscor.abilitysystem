#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Get Ability")]
    [UnitSubtitle("VisualScriptingAbility Unit")]
    [UnitCategory("StudioScor\\AbilitySystem\\AbilitySpec")]
    public class VisualScriptingAbilityGetAbilityUnit : Unit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput AbilitySpec { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySystem")]
        [PortLabelHidden]
        public ValueOutput Ability { get; private set; }


        protected override void Definition()
        {
            AbilitySpec = ValueInput<VisualScriptingAbilitySpec>(nameof(AbilitySpec), null).NullMeansSelf();

            Ability = ValueOutput<Ability>(nameof(Ability), GetAbility);

            Requirement(AbilitySpec, Ability);
        }

        private Ability GetAbility(Flow flow)
        {
            var spec = flow.GetValue<VisualScriptingAbilitySpec>(AbilitySpec);

            return spec.Ability;
        }
    }
}

#endif