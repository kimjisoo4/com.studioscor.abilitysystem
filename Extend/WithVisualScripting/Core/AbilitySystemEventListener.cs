
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    public sealed class AbilitySpecEventListener : MessageListener
    {
        private void Start()
        {
            var abilitySpec = GetComponent<IAbilitySpec>();

            abilitySpec.OnActivatedAbility += AbilitySpec_OnActivatedAbility;
            abilitySpec.OnCanceledAbility += AbilitySpec_OnCanceledAbility;
            abilitySpec.OnFinishedAbility += AbilitySpec_OnFinishedAbility;
            abilitySpec.OnEndedAbility += AbilitySpec_OnEndedAbility;
            abilitySpec.OnChangedAbilityLevel += AbilitySpec_OnChangedAbilityLevel;
        }
        private void OnDestroy()
        {
            var abilitySpec = GetComponent<IAbilitySpec>();

            abilitySpec.OnActivatedAbility -= AbilitySpec_OnActivatedAbility;
            abilitySpec.OnCanceledAbility -= AbilitySpec_OnCanceledAbility;
            abilitySpec.OnFinishedAbility -= AbilitySpec_OnFinishedAbility;
            abilitySpec.OnEndedAbility -= AbilitySpec_OnEndedAbility;
            abilitySpec.OnChangedAbilityLevel -= AbilitySpec_OnChangedAbilityLevel;
        }
        private void AbilitySpec_OnFinishedAbility(IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_FINISHED_ABILITY, gameObject);
        }

        private void AbilitySpec_OnEndedAbility(IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ENDED_ABILITY, gameObject, abilitySpec);
        }

        private void AbilitySpec_OnChangedAbilityLevel(IAbilitySpec abilitySpec, int currentLevel, int prevLevel)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CHANGED_ABILITY_LEVEL, gameObject, prevLevel);
        }

        private void AbilitySpec_OnCanceledAbility(IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCELED_ABILITY, gameObject);
        }

        private void AbilitySpec_OnActivatedAbility(IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ACTIVATED_ABILITY, gameObject);
        }

        
    }
    public sealed class AbilitySystemEventListener : MessageListener
    {
        private void Start()
        {
            var abilitySystem = GetComponent<AbilitySystemComponent>();

            abilitySystem.OnGrantedAbility += AbilitySystem_OnGrantedAbility;
            abilitySystem.OnRemovedAbility += AbilitySystem_OnRemovedAbility;
            abilitySystem.OnActivatedAbility += AbilitySystem_OnActivatedAbility;
            abilitySystem.OnFinishedAbility += AbilitySystem_OnFinishedAbility;
        }
        private void OnDestroy()
        {
            var abilitySystem = GetComponent<AbilitySystemComponent>();

            abilitySystem.OnGrantedAbility -= AbilitySystem_OnGrantedAbility;
            abilitySystem.OnRemovedAbility -= AbilitySystem_OnRemovedAbility;
            abilitySystem.OnActivatedAbility -= AbilitySystem_OnActivatedAbility;
            abilitySystem.OnFinishedAbility -= AbilitySystem_OnFinishedAbility;
        }

        private void AbilitySystem_OnFinishedAbility(AbilitySystemComponent abilitySystemComponent, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_FINISH_ABILITY, gameObject, abilitySpec);
        }

        private void AbilitySystem_OnActivatedAbility(AbilitySystemComponent abilitySystemComponent, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_ACTIVE_ABILITY, gameObject, abilitySpec);
        }

        private void AbilitySystem_OnRemovedAbility(AbilitySystemComponent abilitySystemComponent, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_REMOVE_ABILITY, gameObject, abilitySpec);
        }

        private void AbilitySystem_OnGrantedAbility(AbilitySystemComponent abilitySystemComponent, IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(AbilitySystemWithVisualScriptingEvent.ABILITYSYSTEM_GRANT_ABILITY, gameObject, abilitySpec);
        }
    }
}
#endif