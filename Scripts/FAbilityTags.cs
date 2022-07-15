using UnityEngine;

namespace KimScor.GameplayTagSystem.Ability
{
    [System.Serializable]
    public struct FAbilityTags
    {
        /// <summary>
        /// 어빌리티 사용시 부여되는 태그
        /// </summary>
        [Header(" 활성화 부여 태그 ")]
        public GameplayTag[] ActivationOwnedTags;

        /// <summary>
        /// 어빌리티 차단 태그를 부여한다.
        /// </summary>
        [Header(" 어빌리티 차단 태그 ")]
        public GameplayTag[] BlockAbilitiesWithTag;

        /// <summary>
        /// 어빌리티를 확인하여 해당 태그를 취소한다.
        /// </summary>
        [Header(" 어빌리티 취소 태그 ")]
        public GameplayTag[] CancelAbilitiesWithTag;

        /// <summary>
        /// 어빌리티를 실행할 때, 해당 태그들이 모두 존재해야 한다.
        /// </summary>
        [Header("활성화 필수 태그")]
        public GameplayTag[] ActivationRequiredTags;
        /// <summary>
        /// 어빌리티를 실행할 때, 해당 태그들이 모두 존재하지 않아야 한다.
        /// </summary>
        [Header("활성화 차단 태그")]
        public GameplayTag[] ActivationBlockedTags;

        public FAbilityTags(GameplayTag abilityTag, GameplayTag[] attributeTags, GameplayTag[] activationOwnedTags, GameplayTag[] blockAbilitiesWithTag, GameplayTag[] cancelAbilitiesWithTag, GameplayTag[] activationRequiredTags, GameplayTag[] activationBlockedTags)
        {
            ActivationOwnedTags = activationOwnedTags;
            BlockAbilitiesWithTag = blockAbilitiesWithTag;
            CancelAbilitiesWithTag = cancelAbilitiesWithTag;
            ActivationRequiredTags = activationRequiredTags;
            ActivationBlockedTags = activationBlockedTags;
        }
    }
}
