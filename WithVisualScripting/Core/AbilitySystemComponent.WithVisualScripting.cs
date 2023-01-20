using UnityEngine;
using System.Diagnostics;

#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
#endif


namespace StudioScor.AbilitySystem
{
    public partial class AbilitySystemComponent : MonoBehaviour
    {
        private partial void Callback_OnActivatedAbilityWithVisualScripting(IAbilitySpec abilitySpec)
        {
#if SCOR_ENABLE_VISUALSCRIPTING
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.ABILITYSYSTEM_ACTIVE_ABILITY, gameObject, abilitySpec);
#endif
        }
        private partial void Callback_OnFinishedAbilityWithVisualScripting(IAbilitySpec abilitySpec)
        {
#if SCOR_ENABLE_VISUALSCRIPTING
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.ABILITYSYSTEM_FINISH_ABILITY, gameObject, abilitySpec);
#endif
        }
        private partial void Callback_OnGrantedAbilityWithVisualScripting(IAbilitySpec abilitySpec)
        {
#if SCOR_ENABLE_VISUALSCRIPTING
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.ABILITYSYSTEM_GRANT_ABILITY, gameObject, abilitySpec);
#endif
        }
        private partial void Callback_OnRemovedAbilityWithVisualScripting(IAbilitySpec abilitySpec)
        {
#if SCOR_ENABLE_VISUALSCRIPTING
            EventBus.Trigger(AbilitySystemVisualScriptingEvent.ABILITYSYSTEM_REMOVE_ABILITY, gameObject, abilitySpec);
#endif
        }
    }
}
