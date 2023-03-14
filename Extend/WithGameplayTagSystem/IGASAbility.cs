#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public interface IGASAbility
    {
        public GameplayTag AbilityTag { get; }
        public IReadOnlyCollection<GameplayTag> AttributeTags { get; }
        public IReadOnlyCollection<GameplayTag> CancelAbilityTags { get; }
        public FGameplayTags GrantTags { get; }
        public FConditionTags ConditionTags { get; }
    }
}
#endif