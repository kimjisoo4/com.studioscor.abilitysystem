
#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using UnityEngine;
using Unity.VisualScripting;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    public abstract class AbilitySpecEventUnit : CustomEventUnit<GameObjectAbilitySpec, EmptyEventArgs>
    {
        public override Type MessageListenerType => typeof(AbilitySpecMessageListener);
    }
}

#endif