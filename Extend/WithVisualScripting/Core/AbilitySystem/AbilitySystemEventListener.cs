
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
            var abilitySystem = GetComponent<IAbilitySystem>();

            abilitySystem.OnGrantedAbility += AbilitySystem_OnGrantedAbility;
            abilitySystem.OnRemovedAbility += AbilitySystem_OnRemovedAbility;
            abilitySystem.OnActivatedAbility += AbilitySystem_OnActivatedAbility;
            abilitySystem.OnEndedAbility += AbilitySystem_OnEndedAbility;
            abilitySystem.OnReleasedAbility += AbilitySystem_OnReleasedAbility;
        }

        

        private void OnDestroy()
        {
            if(TryGetComponent(out IAbilitySystem abilitySystem))
            {
                abilitySystem.OnGrantedAbility -= AbilitySystem_OnGrantedAbility;
                abilitySystem.OnRemovedAbility -= AbilitySystem_OnRemovedAbility;
                abilitySystem.OnActivatedAbility -= AbilitySystem_OnActivatedAbility;
                abilitySystem.OnEndedAbility -= AbilitySystem_OnEndedAbility;
                abilitySystem.OnReleasedAbility -= AbilitySystem_OnReleasedAbility;
            }
        }
        private void AbilitySystem_OnActivatedAbility(IAbilitySystem abilitySystem, IAbilitySpecEvent abilitySpecEvent)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_ACTIVE_ABILITY, abilitySystem), abilitySpecEvent);
        }
        private void AbilitySystem_OnReleasedAbility(IAbilitySystem abilitySystem, IAbilitySpecEvent abilitySpecEvent)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_RELEASE_ABILITY, abilitySystem), abilitySpecEvent);
        }
        private void AbilitySystem_OnEndedAbility(IAbilitySystem abilitySystem, IAbilitySpecEvent abilitySpecEvent)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_END_ABILITY, abilitySystem), abilitySpecEvent);
        }
        private void AbilitySystem_OnRemovedAbility(IAbilitySystem abilitySystem, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_REMOVE_ABILITY, abilitySystem), abilitySpec);
        }

        private void AbilitySystem_OnGrantedAbility(IAbilitySystem abilitySystem, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_GRANT_ABILITY, abilitySystem), abilitySpec);
        }
    }
}
#endif