
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    public abstract class VisualScriptingAbilitySpecLevelEventUnit : CustomGameObjectEventUnit<VisualScriptingAbilitySpec, int>
    {
        [DoNotSerialize]
        [PortLabel("Level")]
        public ValueOutput Level { get; private set; }

        public override Type MessageListenerType => null;

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