
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{

    [UnitTitle("Try Grant Ability")]
    [UnitSubtitle("AbilitySystem Unit")]
    public class AbilitySystemTryGrantAbilityUnit : AbilitySystemFlowUnit
    {
        [DoNotSerialize]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        [DoNotSerialize]
        [PortLabel("Level")]
        [PortLabelHidden]
        public ValueInput Level { get; private set; }

        [DoNotSerialize]
        [PortLabel("isGrant")]
        [PortLabelHidden]
        public ValueOutput IsGrant { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySpec")]
        [PortLabelHidden]
        public ValueOutput AbilitySpec { get; private set; }

        protected override void Definition()
        {
            base.Definition();

            Ability = ValueInput<Ability>(nameof(Ability), null);
            Level = ValueInput<int>(nameof(Level), 1);

            AbilitySpec = ValueOutput<IAbilitySpec>(nameof(AbilitySpec));
            IsGrant = ValueOutput<bool>(nameof(IsGrant));

            Requirement(Target, AbilitySpec);
            Requirement(Target, IsGrant);

            Requirement(Ability, Enter);
            Requirement(Ability, AbilitySpec);
            Requirement(Ability, IsGrant);

            Requirement(Level, Enter);
            Requirement(Level, AbilitySpec);
            Requirement(Level, IsGrant);
        }

        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySystem = flow.GetValue<IAbilitySystem>(Target);
            var ability = flow.GetValue<Ability>(Ability);
            var level = flow.GetValue<int>(Level);

            var result = abilitySystem.TryGrantAbility(ability, level, out IAbilitySpec spec);

            flow.SetValue(AbilitySpec, spec);
            flow.SetValue(IsGrant, result);

            return Exit;
        }
    }
}

#endif