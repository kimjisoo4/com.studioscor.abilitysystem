
#if SCOR_ENABLE_VISUALSCRIPTING
using System.Collections.Generic;
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Get Abilities")]
    [UnitSubtitle("AbilitySystem Unit")]
    public class AbilitySystemGetAbilitiesUnit : AbilitySystemUnit
    {
        [DoNotSerialize]
        [PortLabel("Abilities")]
        [PortLabelHidden]
        public ValueOutput Abilities { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Abilities = ValueOutput<IReadOnlyDictionary<IAbility, IAbilitySpec>>(nameof(Abilities), GetAbilities);

            Requirement(Target, Abilities);
        }

        private IReadOnlyDictionary<IAbility, IAbilitySpec> GetAbilities(Flow flow)
        {
            return flow.GetValue<IAbilitySystem>(Target).Abilities;
        }
    }
}

#endif