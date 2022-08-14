using UnityEngine;

namespace KimScor.GameplayTagSystem.Ability
{
    [System.Serializable]
    public struct FAbilityTags
    {
        /// <summary>
        /// 부여 소유 태그
        /// </summary>
        [Header(" 활성화 부여 태그 ")]
        public GameplayTag[] AddOwnedTags;

        /// <summary>
        /// 부여 차단 태그
        /// </summary>
        [Header(" 어빌리티 차단 태그 ")]
        public GameplayTag[] AddBlockTags;

        /// <summary>
        /// 캔슬 태그
        /// </summary>
        [Header(" 어빌리티 취소 태그 ")]
        public GameplayTag[] CancelAbilityTags;

        /// <summary>
        /// 필요 태그
        /// </summary>
        [Header("활성화 필수 태그")]
        public GameplayTag[] RequiredTags;
        /// <summary>
        /// 방해 태그
        /// </summary>
        [Header("활성화 차단 태그")]
        public GameplayTag[] ObstacledTags;

        public FAbilityTags(GameplayTag abilityTag, GameplayTag[] attributeTags, GameplayTag[] activationOwnedTags, GameplayTag[] blockAbilitiesWithTag, GameplayTag[] cancelAbilitiesWithTag, GameplayTag[] activationRequiredTags, GameplayTag[] activationBlockedTags)
        {
            AddOwnedTags = activationOwnedTags;
            AddBlockTags = blockAbilitiesWithTag;
            CancelAbilityTags = cancelAbilitiesWithTag;
            RequiredTags = activationRequiredTags;
            ObstacledTags = activationBlockedTags;
        }
    }
}
