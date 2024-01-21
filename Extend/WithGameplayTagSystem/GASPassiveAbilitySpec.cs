#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract class GASPassiveAbilitySpec : GASAbilitySpec
    {
        protected GASPassiveAbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level) : base(ability, abilitySystem, level)
        {
        }

        protected override void OnGrantAbility()
        {
            base.OnGrantAbility();

            var gameplayTagSystem = _AbilitySystem.gameObject.GetGameplayTagSystem();

            gameplayTagSystem.OnGrantedOwnedTag += GameplayTagEvent_OnGrantedOwnedTag;
            gameplayTagSystem.OnGrantedBlockTag += GameplayTagEvent_OnGrantedBlockTag;
            gameplayTagSystem.OnRemovedOwnedTag += GameplayTagEvent_OnRemovedOwnedTag;
            gameplayTagSystem.OnRemovedBlockTag += GameplayTagEvent_OnRemovedBlockTag;

            TryActiveAbility();
        }
        protected override void OnRemoveAbility()
        {
            base.OnRemoveAbility();

            ForceEndAbility();

            var gameplayTagSystem = _AbilitySystem.gameObject.GetGameplayTagSystem();

            if (gameplayTagSystem is not null)
            {
                gameplayTagSystem.OnGrantedOwnedTag -= GameplayTagEvent_OnGrantedOwnedTag;
                gameplayTagSystem.OnGrantedBlockTag -= GameplayTagEvent_OnGrantedBlockTag;
                gameplayTagSystem.OnRemovedOwnedTag -= GameplayTagEvent_OnRemovedOwnedTag;
                gameplayTagSystem.OnRemovedBlockTag -= GameplayTagEvent_OnRemovedBlockTag;
            }
        }

        #region Auto Toggle
        private void GameplayTagEvent_OnGrantedOwnedTag(IGameplayTagSystem gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (IsPlaying)
            {
                if (_Ability.ConditionTags.Obstacleds.Contains(gameplayTag))
                {
                    TryEndAbility();
                }
            }
            else
            {
                if (_Ability.ConditionTags.Requireds.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        private void GameplayTagEvent_OnRemovedOwnedTag(IGameplayTagSystem gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (IsPlaying)
            {
                if (_Ability.ConditionTags.Requireds.Contains(gameplayTag))
                {
                    TryEndAbility();
                }
            }
            else
            {
                if (_Ability.ConditionTags.Obstacleds.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        private void GameplayTagEvent_OnGrantedBlockTag(IGameplayTagSystem gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (IsPlaying)
            {
                if (gameplayTag == _Ability.AbilityTag || _Ability.AttributeTags.Contains(gameplayTag))
                {
                    TryEndAbility();
                }
            }
        }
        private void GameplayTagEvent_OnRemovedBlockTag(IGameplayTagSystem gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (!IsPlaying)
            {
                if (gameplayTag == _Ability.AbilityTag || _Ability.AttributeTags.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        #endregion
    }
}
#endif