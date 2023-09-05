#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Collections.Generic;
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract class GASPassiveAbilitySpec : GASAbilitySpec
    {
        private readonly GameplayTagConditionObserver gameplayTagConditionObserver;

        protected GASPassiveAbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level) : base(ability, abilitySystem, level)
        {
            var gameplayTagSystem = abilitySystem.gameObject.GetGameplayTagSystem();

            List<GameplayTag> attributeTags = new();

            if(this.ability.AbilityTag)
                attributeTags.Add(this.ability.AbilityTag);

            attributeTags.AddRange(this.ability.AttributeTags);

            gameplayTagConditionObserver = new(gameplayTagSystem, attributeTags, this.ability.ConditionTags);
        }

        protected override void OnGrantAbility()
        {
            base.OnGrantAbility();

            gameplayTagConditionObserver.OnChangedState += GameplayTagConditionObserver_OnChangedState;
            gameplayTagConditionObserver.OnObserver();

            TryActiveAbility();
        }

        protected override void OnRemoveAbility()
        {
            base.OnRemoveAbility();

            gameplayTagConditionObserver.OnChangedState -= GameplayTagConditionObserver_OnChangedState;
            gameplayTagConditionObserver.EndObserver();

            ForceEndAbility();
        }

        private void GameplayTagConditionObserver_OnChangedState(GameplayTagObserver gameplayTagConditionObserver, bool isOn)
        {
            if (isOn)
            {
                TryActiveAbility();
            }
            else
            {
                TryEndAbility();
            }
        }

    }
}
#endif