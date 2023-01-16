using UnityEngine;

#if SCOR_ENABLE_GAMEPLAYTAG
using StudioScor.GameplayTagSystem;
#endif

namespace StudioScor.AbilitySystem
{
#if SCOR_ENABLE_GAMEPLAYTAG
    [System.Serializable]
    public struct FAbilityTags
    {
        [Header("조건 태그")]
        public FConditionTags ConditionTags;

        [Header(" 부여 태그 ")]
        public FGameplayTags GrantTags;

        [Header(" 어빌리티 취소 태그 ")]
        public GameplayTag[] CancelAbilityTags;
    }
#endif
}
