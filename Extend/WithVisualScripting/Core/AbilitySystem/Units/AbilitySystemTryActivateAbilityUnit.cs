
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Try Activate Ability")]
    [UnitSubtitle("AbilitySystem Unit")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemTryActivateAbilityUnit : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput Enter { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput Exit { get; private set; }

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Target")]
        [PortLabelHidden]
        public ValueInput AbilitySystem { get; private set; }

        [DoNotSerialize]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        [DoNotSerialize]
        [PortLabel("isActivate")]
        [PortLabelHidden]
        public ValueOutput IsActivate { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySpec")]
        [PortLabelHidden]
        public ValueOutput AbilitySpec { get; private set; }

        protected override void Definition()
        {
            Enter = ControlInput(nameof(Enter), TryActivateAbility);
            Exit = ControlOutput(nameof(Exit));

            AbilitySystem = ValueInput<GameObject>(nameof(AbilitySystem), null).NullMeansSelf();
            Ability = ValueInput<Ability>(nameof(Ability));

            AbilitySpec = ValueOutput<IAbilitySpec>(nameof(Ability));
            IsActivate = ValueOutput<bool>(nameof(IsActivate));

            Requirement(AbilitySystem, AbilitySpec);
            Requirement(AbilitySystem, IsActivate);
            Requirement(Ability, AbilitySpec);
            Requirement(Ability, IsActivate);

            Succession(Enter, Exit);
        }

        private ControlOutput TryActivateAbility(Flow flow)
        {
            var abilitySystem = flow.GetValue<IAbilitySystem>(AbilitySystem);
            var ability = flow.GetValue<Ability>(Ability);

            var result = abilitySystem.TryActivateAbility(ability);

            flow.SetValue(AbilitySpec, result.abilitySpec);
            flow.SetValue(IsActivate, result.isActivate);

            return Exit;
        }
    }
}

#endif