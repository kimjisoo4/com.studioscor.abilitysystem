#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using UnityEngine;
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract class GASAbility : Ability, IGASAbility
    {
        [Header(" [ Gameplay Ability ] ")]
        [SerializeField] private GameplayTag _AbilityTag;
        [SerializeField] private GameplayTag[] _AttributeTags;
        [SerializeField] private FGameplayTags _GrantTags;
        [SerializeField] private FConditionTags _ConditionTags;
        [SerializeField] private GameplayTag[] _CancelAbilityTags;

        public GameplayTag AbilityTag => _AbilityTag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => _AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelAbilityTags => _CancelAbilityTags;
        public FGameplayTags GrantTags => _GrantTags;
        public FConditionTags ConditionTags => _ConditionTags;
    }
}
#endif