
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    public abstract class AbilitySpecLevelCustomEventUnit : GameObjectEventUnit<int>
    {
        [DoNotSerialize]
        public ValueOutput Level { get; private set; }

        public override Type MessageListenerType => default;
        protected abstract string EventName { get; }

        protected override string hookName => EventName;

        protected override void Definition()
        {
            base.Definition();

            Level = ValueOutput<int>(nameof(Level));
        }

        protected override void AssignArguments(Flow flow, int data)
        {
            flow.SetValue(Level, data);
        }
    }
}

#endif