
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    public abstract class AbilitySystemCustomEventUnit : GameObjectEventUnit<IAbilitySpec>
    {
        [DoNotSerialize]
        [PortLabel("Owner")]
        public ValueOutput AbilitySystemComponent { get; private set; }

        [DoNotSerialize]
        [PortLabel("Ability")]
        public ValueOutput Ability { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySpec")]
        public ValueOutput AbilitySpec { get; private set; }


        public override Type MessageListenerType => typeof(AbilitySystemEventListener);

        protected override void Definition()
        {
            base.Definition();

            AbilitySystemComponent = ValueOutput<AbilitySystemComponent>(nameof(AbilitySystemComponent));
            Ability = ValueOutput<Ability>(nameof(Ability));
            AbilitySpec = ValueOutput<IAbilitySpec>(nameof(AbilitySpec));
        }

        protected override void AssignArguments(Flow flow, IAbilitySpec data)
        {
            flow.SetValue(AbilitySystemComponent, data.AbilitySystemComponent);
            flow.SetValue(Ability, data.Ability);
            flow.SetValue(AbilitySpec, data);
        }
    }
}

#endif