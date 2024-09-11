#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract class GASGameObjectPassiveAbilitySpec : GASGameObjectAbilitySpec
    {

        protected override void OnGrantAbility()
        {
            base.OnGrantAbility();

            var gameplayTagSystem = _abilitySystem.gameObject.GetGameplayTagSystem();

            gameplayTagSystem.OnGrantedOwnedTag += GameplayTagEvent_OnGrantedOwnedTag;
            gameplayTagSystem.OnGrantedBlockTag += GameplayTagEvent_OnGrantedBlockTag;
            gameplayTagSystem.OnRemovedOwnedTag += GameplayTagEvent_OnRemovedOwnedTag;
            gameplayTagSystem.OnRemovedBlockTag += GameplayTagEvent_OnRemovedBlockTag;

            TryActiveAbility();
        }
        protected override void OnRemoveAbility()
        {
            base.OnRemoveAbility();

            ForceFinishAbility();

            var gameplayTagSystem = _abilitySystem.gameObject.GetGameplayTagSystem();

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
                if (_gasAbility.ConditionTags.Obstacleds.Contains(gameplayTag))
                {
                    TryFinishAbility();
                }
            }
            else
            {
                if (_gasAbility.ConditionTags.Requireds.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        private void GameplayTagEvent_OnRemovedOwnedTag(IGameplayTagSystem gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (IsPlaying)
            {
                if (_gasAbility.ConditionTags.Requireds.Contains(gameplayTag))
                {
                    TryFinishAbility();
                }
            }
            else
            {
                if (_gasAbility.ConditionTags.Obstacleds.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        private void GameplayTagEvent_OnGrantedBlockTag(IGameplayTagSystem gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (IsPlaying)
            {
                if (gameplayTag == _gasAbility.AbilityTag || _gasAbility.AttributeTags.Contains(gameplayTag))
                {
                    TryFinishAbility();
                }
            }
        }
        private void GameplayTagEvent_OnRemovedBlockTag(IGameplayTagSystem gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (!IsPlaying)
            {
                if (gameplayTag == _gasAbility.AbilityTag || _gasAbility.AttributeTags.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        #endregion
    }
}
#endif