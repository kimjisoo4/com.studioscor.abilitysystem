
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using UnityEngine;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem
{
    public abstract class AbilitySpecCustomEventUnit : GameObjectEventUnit<EmptyEventArgs>
    {
        public override Type MessageListenerType => default;
        protected abstract string EventName { get; }
        protected override string hookName => EventName;
    }
}

#endif