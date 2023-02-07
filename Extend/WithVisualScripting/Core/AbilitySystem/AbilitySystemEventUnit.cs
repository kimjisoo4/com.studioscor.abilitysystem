
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    public abstract class AbilitySystemEventUnit : CustomEventUnit<AbilitySystemComponent, IAbilitySpec>
    {
        [DoNotSerialize]
        [PortLabel("Ability")]
        public ValueOutput Ability { get; private set; }

        [DoNotSerialize]
        [PortLabel("AbilitySpec")]
        public ValueOutput AbilitySpec { get; private set; }

        public override Type MessageListenerType => typeof(AbilitySystemMessageListener);

        protected override void Definition()
        {
            base.Definition();

            Ability = ValueOutput<Ability>(nameof(Ability));
            AbilitySpec = ValueOutput<IAbilitySpec>(nameof(AbilitySpec));
        }

        protected override void AssignArguments(Flow flow, IAbilitySpec data)
        {
            flow.SetValue(Ability, data.Ability);
            flow.SetValue(AbilitySpec, data);
        }
    }
}

#endif