#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Get Ability Level")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecGetAbilityLevelUnit : AbilitySpecUnit
    {
        [DoNotSerialize]
        [PortLabel("Level")]
        [PortLabelHidden]
        public ValueOutput Level { get; private set; }


        protected override void Definition()
        {
            base.Definition();

            Level = ValueOutput<int>(nameof(Level), GetAbility);

            Requirement(Target, Level);
        }

        private int GetAbility(Flow flow)
        {
            var spec = flow.GetValue<IAbilitySpec>(Target);

            return spec.Level;
        }
    }
}

#endif