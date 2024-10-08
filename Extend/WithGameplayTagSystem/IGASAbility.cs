#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public interface IGASAbility
    {
        public GameplayTagSO AbilityTag { get; }
        public IReadOnlyCollection<GameplayTagSO> AttributeTags { get; }
        public IReadOnlyCollection<GameplayTagSO> CancelAbilityTags { get; }
        public FGameplayTags GrantTags { get; }
        public FConditionTags ConditionTags { get; }

        IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0);
    }
}
#endif