#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using Unity.VisualScripting;
using StudioScor.Utilities.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{

    public abstract class VisualScriptingAbilitySpecEventUnit : CustomEventUnit<VisualScriptingAbilitySpec, EmptyEventArgs>
    {
        public override Type MessageListenerType => null;
    }
}

#endif