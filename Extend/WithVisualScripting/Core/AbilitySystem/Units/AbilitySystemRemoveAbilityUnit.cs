
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{

    [UnitTitle("Remove Ability")]
    [UnitSubtitle("AbilitySystem Unit")]
    public class AbilitySystemRemoveAbilityUnit : AbilitySystemFlowUnit
    {
        [DoNotSerialize]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        [DoNotSerialize]
        [PortLabel("isRemove")]
        [PortLabelHidden]
        public ValueOutput IsRemove { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySpec")]
        [PortLabelHidden]
        public ValueOutput AbilitySpec { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Ability = ValueInput<Ability>(nameof(Ability), null);

            AbilitySpec = ValueOutput<IAbilitySpec>(nameof(Ability));
            IsRemove = ValueOutput<bool>(nameof(IsRemove));

            Requirement(Target, AbilitySpec);
            Requirement(Target, IsRemove);

            Requirement(Ability, Enter);
            Requirement(Ability, AbilitySpec);
            Requirement(Ability, IsRemove);
        }

        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySystem = flow.GetValue<IAbilitySystem>(Target);
            var ability = flow.GetValue<Ability>(Ability);

            var result = abilitySystem.RemoveAbility(ability);

            flow.SetValue(IsRemove, result);

            return Exit;
        }
    }
}

#endif