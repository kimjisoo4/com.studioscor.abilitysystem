﻿
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Cancel Ability")]
    [UnitSubtitle("AbilitySystem Unit")]
    public class AbilitySystemCancelAbilityUnit : AbilitySystemFlowUnit
    {
        [DoNotSerialize]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Ability = ValueInput<Ability>(nameof(Ability));
            Requirement(Ability, Enter);
        }

        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySystem = flow.GetValue<IAbilitySystem>(Target);
            var ability = flow.GetValue<Ability>(Ability);

            abilitySystem.CancelAbility(ability);

            return Exit;
        }
    }
}

#endif