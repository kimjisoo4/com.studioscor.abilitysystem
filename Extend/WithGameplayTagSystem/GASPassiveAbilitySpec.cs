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

            var gameplayTagEvent = abilitySystem.gameObject.GetGameplayTagSystemEvent();

            gameplayTagEvent.OnGrantedOwnedTag += GameplayTagEvent_OnGrantedOwnedTag;
            gameplayTagEvent.OnGrantedBlockTag += GameplayTagEvent_OnGrantedBlockTag;
            gameplayTagEvent.OnRemovedOwnedTag += GameplayTagEvent_OnRemovedOwnedTag;
            gameplayTagEvent.OnRemovedBlockTag += GameplayTagEvent_OnRemovedBlockTag;

            TryActiveAbility();
        }
        protected override void OnRemoveAbility()
        {
            base.OnRemoveAbility();

            EndAbility();

            var gameplayTagEvent = abilitySystem.gameObject.GetGameplayTagSystemEvent();

            if (gameplayTagEvent is not null)
            {
                gameplayTagEvent.OnGrantedOwnedTag -= GameplayTagEvent_OnGrantedOwnedTag;
                gameplayTagEvent.OnGrantedBlockTag -= GameplayTagEvent_OnGrantedBlockTag;
                gameplayTagEvent.OnRemovedOwnedTag -= GameplayTagEvent_OnRemovedOwnedTag;
                gameplayTagEvent.OnRemovedBlockTag -= GameplayTagEvent_OnRemovedBlockTag;
            }
        }

        #region Auto Toggle
        private void GameplayTagEvent_OnGrantedOwnedTag(IGameplayTagSystemEvent gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (isPlaying)
            {
                if (ability.ConditionTags.Obstacleds.Contains(gameplayTag))
                {
                    EndAbility();
                }
            }
            else
            {
                if (ability.ConditionTags.Requireds.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        private void GameplayTagEvent_OnRemovedOwnedTag(IGameplayTagSystemEvent gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (IsPlaying)
            {
                if (ability.ConditionTags.Requireds.Contains(gameplayTag))
                {
                    EndAbility();
                }
            }
            else
            {
                if (ability.ConditionTags.Obstacleds.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        private void GameplayTagEvent_OnGrantedBlockTag(IGameplayTagSystemEvent gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (IsPlaying)
            {
                if (gameplayTag == ability.AbilityTag || ability.AttributeTags.Contains(gameplayTag))
                {
                    EndAbility();
                }
            }
        }
        private void GameplayTagEvent_OnRemovedBlockTag(IGameplayTagSystemEvent gameplayTagSystem, GameplayTag gameplayTag)
        {
            if (!IsPlaying)
            {
                if (gameplayTag == ability.AbilityTag || ability.AttributeTags.Contains(gameplayTag))
                {
                    TryActiveAbility();
                }
            }
        }
        #endregion
    }
}
#endif