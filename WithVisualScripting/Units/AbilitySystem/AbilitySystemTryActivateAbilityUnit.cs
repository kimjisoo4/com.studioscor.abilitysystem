#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using StudioScor.GameplayTagSystem;


namespace StudioScor.AbilitySystem
{

    [UnitTitle("TestUnit")]
    public class TestUnit : Unit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput Enter;

        [DoNotSerialize]
        [PortLabel("Exit")]
        [PortLabelHidden]
        public ControlOutput Exit;

        [DoNotSerialize]
        [PortLabel("Failed")]
        public ControlOutput Event;

        [DoNotSerialize]
        public ValueInput Duration { get; private set; }

        protected override void Definition()
        {
            Enter = ControlInputCoroutine(nameof(Enter), TEST);

            Exit = ControlOutput(nameof(Exit));
            Event = ControlOutput(nameof(Event));

            Duration = ValueInput<float>(nameof(Duration), 0.2f);
        }

        IEnumerator TEST(Flow flow)
        {
            yield return Exit;

            var duration = flow.GetValue<float>(Duration);

            yield return new WaitForSeconds(duration);

            yield return Event;
        }
    }


    [UnitTitle("TryActivateAbility")]
    [UnitCategory("StudioScor\\AbilitySystem")]
    public class AbilitySystemTryActivateAbilityUnit : AbilitySystemUnit
    {
        [DoNotSerialize]
        [PortLabel("Enter")]
        [PortLabelHidden]
        public ControlInput Enter;

        [DoNotSerialize]
        [PortLabel("Successed")]
        public ControlOutput Commited;

        [DoNotSerialize]
        [PortLabel("Failed")]
        public ControlOutput Failed;

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("Ability")]
        [PortLabelHidden]
        public ValueInput Ability { get; private set; }

        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("IsCommit")]
        [PortLabelHidden]
        public ValueOutput IsCommit { get; private set; }

        private bool _IsCommit;

        protected override void Definition()
        {
            base.Definition();

            Enter = ControlInput(nameof(Enter), TryActivateAbility);

            Commited = ControlOutput(nameof(Commited));
            Failed = ControlOutput(nameof(Failed));

            Ability = ValueInput<Ability>(nameof(Ability), null);

            IsCommit = ValueOutput<bool>(nameof(IsCommit), (flow) => { return _IsCommit; });
        }

        private ControlOutput TryActivateAbility(Flow flow)
        {
            var abilitySystemComponent = flow.GetValue<AbilitySystemComponent>(AbilitySystemComponent);
            var ability = flow.GetValue<Ability>(Ability);

            _IsCommit = abilitySystemComponent.TryActivateAbility(ability);

            return _IsCommit ? Commited : Failed;
        }
    }
}

#endif