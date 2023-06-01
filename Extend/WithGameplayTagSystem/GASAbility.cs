#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using UnityEngine;
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract class GASAbility : Ability, IGASAbility
    {
        [Header(" [ Gameplay Ability ] ")]
        [SerializeField] private GameplayTag abilityTag;
        [SerializeField] private GameplayTag[] attributeTags;
        [SerializeField] private FGameplayTags grantTags;
        [SerializeField] private FConditionTags conditionTags;
        [SerializeField] private GameplayTag[] cancelAbilityTags;

        public GameplayTag AbilityTag => abilityTag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => attributeTags;
        public IReadOnlyCollection<GameplayTag> CancelAbilityTags => cancelAbilityTags;
        public FGameplayTags GrantTags => grantTags;
        public FConditionTags ConditionTags => conditionTags;
    }
}
#endif