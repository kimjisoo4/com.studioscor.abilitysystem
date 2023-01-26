#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using System;


namespace StudioScor.AbilitySystem.VisualScripting
{
    public abstract class AbilitySpecEventUnit : GameObjectEventUnit<IAbilitySpec>
    {
        public override Type MessageListenerType => typeof(AbilitySpecMessageListener);
    }
}

#endif