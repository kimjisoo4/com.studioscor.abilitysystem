
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    public abstract class AbilitySpecCustomEventUnit : GameObjectEventUnit<EmptyEventArgs>
    {
        public override Type MessageListenerType => default;
    }
}

#endif