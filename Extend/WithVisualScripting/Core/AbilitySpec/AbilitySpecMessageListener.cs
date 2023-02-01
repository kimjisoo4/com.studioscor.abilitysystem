
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
        private OnChangedLevelValue _OnChangedLevelValue;

        private void Awake()
        {
            var abilitySpec = GetComponent<GameObjectAbilitySpec>();

            abilitySpec.OnActivatedAbility += AbilitySpec_OnActivatedAbility;
            abilitySpec.OnCanceledAbility += AbilitySpec_OnCanceledAbility;
            abilitySpec.OnFinishedAbility += AbilitySpec_OnFinishedAbility;
            abilitySpec.OnEndedAbility += AbilitySpec_OnEndedAbility;
            abilitySpec.OnChangedAbilityLevel += AbilitySpec_OnChangedAbilityLevel;
        }
        private void OnDestroy()
        {
            if(TryGetComponent(out GameObjectAbilitySpec abilitySpec))
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
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_FINISHED_ABILITY, abilitySpec));
        }

        private void AbilitySpec_OnEndedAbility(IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ENDED_ABILITY, abilitySpec));
        }

        private void AbilitySpec_OnChangedAbilityLevel(IAbilitySpec abilitySpec, int currentLevel, int prevLevel)
        {
            if (_OnChangedLevelValue is null)
                _OnChangedLevelValue = new();

            _OnChangedLevelValue.CurrentLevel = currentLevel;
            _OnChangedLevelValue.PrevLevel = prevLevel;

            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CHANGED_ABILITY_LEVEL, abilitySpec), _OnChangedLevelValue);

            _OnChangedLevelValue.CurrentLevel = default;
            _OnChangedLevelValue.PrevLevel = default;
        }

        private void AbilitySpec_OnCanceledAbility(IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CANCELED_ABILITY, abilitySpec));
        }

        private void AbilitySpec_OnActivatedAbility(IAbilitySpec abilitySpec)
        {
            EventBus.Trigger(new EventHook(AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_ACTIVATED_ABILITY, abilitySpec));
        }

        
    }
}
#endif