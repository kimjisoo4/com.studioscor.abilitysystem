#if SCOR_ENABLE_VISUALSCRIPTING && SCOR_ENABLE_GAMEPLAYTAGSYSTEM

using Unity.VisualScripting;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem.VisualScripting.GameplayTagSystem
{

    [UnitTitle("Check Ability Tags")]
    [UnitSubtitle("Ability System With GameplayTagSystem")]
    [UnitCategory("StudioScor\\AbilitySystem\\WithGameplayTagSystem")]
    public class CheckAbilityTagsUnit : Unit
    {
        [DoNotSerialize]
        [NullMeansSelf]
        [PortLabel("GameplayTagSystem")]
        public ValueInput GameplayTagSystemComponent { get; private set; }

        [DoNotSerialize]
        [PortLabel("AttributeTags")]
        public ValueInput AttributeTags { get; private set; }

        [DoNotSerialize]
        [PortLabel("ObstacledTags")]
        public ValueInput ObstacledTags { get; private set; }

        [DoNotSerialize]
        [PortLabel("RequiredTags")]
        public ValueInput RequiredTags { get; private set; }

        [DoNotSerialize]
        [PortLabel("Condition")]
        [PortLabelHidden]
        public ValueOutput IsPositive;
        protected override void Definition()
        {
            GameplayTagSystemComponent = ValueInput<GameplayTagSystemComponent>(nameof(GameplayTagSystemComponent)).NullMeansSelf();

            AttributeTags = ValueInput<GameplayTag[]>(nameof(AttributeTags), default);
            ObstacledTags = ValueInput<GameplayTag[]>(nameof(ObstacledTags), default);
            RequiredTags = ValueInput<GameplayTag[]>(nameof(RequiredTags), default);

            IsPositive = ValueOutput<bool>(nameof(IsPositive), CheckGameplayTags);

            Requirement(GameplayTagSystemComponent, IsPositive);
            Requirement(AttributeTags, IsPositive);
            Requirement(ObstacledTags, IsPositive);
            Requirement(RequiredTags, IsPositive);
        }

        private bool CheckGameplayTags(Flow flow)
        {
            var gameplayTagSystem = flow.GetValue<GameplayTagSystemComponent>(GameplayTagSystemComponent);

            if (!gameplayTagSystem)
                return false;

            var attributeTags = flow.GetValue<GameplayTag[]>(AttributeTags);
            
            if (attributeTags.Length > 0 && gameplayTagSystem.ContainAnyTagsInBlock(attributeTags))
                return false;

            var obstacledTags = flow.GetValue<GameplayTag[]>(ObstacledTags);

            if (obstacledTags.Length > 0 && gameplayTagSystem.ContainAnyTagsInOwned(obstacledTags))
                return false;

            var requiredTags = flow.GetValue<GameplayTag[]>(RequiredTags);

            if (requiredTags.Length > 0 && !gameplayTagSystem.ContainAllTagsInOwned(requiredTags))
                return false;

            return true;
        }
    }
}
#endif