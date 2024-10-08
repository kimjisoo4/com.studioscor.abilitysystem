#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using UnityEngine;
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract class GASAbility : Ability, IGASAbility
    {
        [Header(" [ Gameplay Ability ] ")]
        [SerializeField] private GameplayTagSO abilityTag;
        [SerializeField] private GameplayTagSO[] attributeTags;
        [SerializeField] private FConditionTags conditionTags;
        [SerializeField] private FGameplayTags grantTags;
        [SerializeField] private GameplayTagSO[] cancelAbilityTags;

        public GameplayTagSO AbilityTag => abilityTag;
        public IReadOnlyCollection<GameplayTagSO> AttributeTags => attributeTags;
        public IReadOnlyCollection<GameplayTagSO> CancelAbilityTags => cancelAbilityTags;
        public FGameplayTags GrantTags => grantTags;
        public FConditionTags ConditionTags => conditionTags;
    }
}
#endif