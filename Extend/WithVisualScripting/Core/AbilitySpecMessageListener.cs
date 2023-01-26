
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
using UnityEngine;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [DisableAnnotation]
    [AddComponentMenu("")]
    [IncludeInSettings(false)]
    public sealed class AbilitySpecMessageListener : MessageListener
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
            if(TryGetComponent(out IAbilitySpec abilitySpec))
            {
                abilitySpec.OnActivatedAbility -= AbilitySpec_OnActivatedAbility;
                abilitySpec.OnCanceledAbility -= AbilitySpec_OnCanceledAbility;
                abilitySpec.OnFinishedAbility -= AbilitySpec_OnFinishedAbility;
                abilitySpec.OnEndedAbility -= AbilitySpec_OnEndedAbility;
                abilitySpec.OnChangedAbilityLevel -= AbilitySpec_OnChangedAbilityLevel;
            }
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
}
#endif