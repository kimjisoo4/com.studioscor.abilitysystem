
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    public abstract class AbilitySpecLevelCustomEventUnit : GameObjectEventUnit<int>
    {
        [DoNotSerialize]
        public ValueOutput Level { get; private set; }
        public override Type MessageListenerType => default;

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