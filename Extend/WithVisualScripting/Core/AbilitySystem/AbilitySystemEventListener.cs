
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [DisableAnnotation]
    [AddComponentMenu("")]
    [IncludeInSettings(false)]
    public sealed class AbilitySystemMessageListener : MessageListener
    {
        private void Awake()
        {
            var abilitySystem = GetComponent<IAbilitySystemEvent>();

            abilitySystem.OnGrantedAbility += AbilitySystem_OnGrantedAbility;
            abilitySystem.OnRemovedAbility += AbilitySystem_OnRemovedAbility;
            abilitySystem.OnActivatedAbility += AbilitySystem_OnActivatedAbility;
            abilitySystem.OnEndedAbility += AbilitySystem_OnEndedAbility;
            abilitySystem.OnReleasedAbility += AbilitySystem_OnReleasedAbility;
        }

        

        private void OnDestroy()
        {
            if(TryGetComponent(out IAbilitySystemEvent abilitySystem))
            {
                abilitySystem.OnGrantedAbility -= AbilitySystem_OnGrantedAbility;
                abilitySystem.OnRemovedAbility -= AbilitySystem_OnRemovedAbility;
                abilitySystem.OnActivatedAbility -= AbilitySystem_OnActivatedAbility;
                abilitySystem.OnEndedAbility -= AbilitySystem_OnEndedAbility;
                abilitySystem.OnReleasedAbility -= AbilitySystem_OnReleasedAbility;
            }
        }
        private void AbilitySystem_OnActivatedAbility(IAbilitySystemEvent abilitySystemEvent, IAbilitySpecEvent abilitySpecEvent)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_ACTIVE_ABILITY, abilitySystemEvent), abilitySpecEvent);
        }
        private void AbilitySystem_OnReleasedAbility(IAbilitySystemEvent abilitySystemEvent, IAbilitySpecEvent abilitySpecEvent)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_RELEASE_ABILITY, abilitySystemEvent), abilitySpecEvent);
        }
        private void AbilitySystem_OnEndedAbility(IAbilitySystemEvent abilitySystemEvent, IAbilitySpecEvent abilitySpecEvent)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_END_ABILITY, abilitySystemEvent), abilitySpecEvent);
        }
        private void AbilitySystem_OnRemovedAbility(IAbilitySystemEvent abilitySystem, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_REMOVE_ABILITY, abilitySystem), abilitySpec);
        }

        private void AbilitySystem_OnGrantedAbility(IAbilitySystemEvent abilitySystem, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_GRANT_ABILITY, abilitySystem), abilitySpec);
        }
    }
}
#endif