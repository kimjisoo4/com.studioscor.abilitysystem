
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    public abstract class AbilitySystemCustomEventUnit : GameObjectEventUnit<IAbilitySpec>
    {
        [DoNotSerialize]
        public ValueOutput AbilitySpec { get; private set; }
        public override Type MessageListenerType => default;
        protected abstract string EventName { get; }
        protected override string hookName => EventName;

        protected override void Definition()
        {
            base.Definition();

            AbilitySpec = ValueOutput<IAbilitySpec>(nameof(AbilitySpec));
        }

        protected override void AssignArguments(Flow flow, IAbilitySpec data)
        {
            flow.SetValue(AbilitySpec, data);
        }
    }
}

#endif