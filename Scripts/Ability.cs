using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class Ability : ScriptableObject
    {
        [Header(" 어빌리티의 이름 ")]
        [SerializeField] protected string _AbilityName;

        [Header(" 어빌리티의 아이콘")]
        [SerializeField] protected Sprite _AbilityIcon;

        [Header(" 어빌리티의 종류")]
        [SerializeField] protected EAbilityType _AbilityType;

        [Header(" 어빌리티의 태그 ")]
        [SerializeField] private GameplayTag _AbilityTag;

        [Header(" 어빌리티의 속성 ")]
        [SerializeField] private GameplayTag[] _AttributeTags;

        [Header(" 어빌리티 취소")]
        [SerializeField] private GameplayTag[] _CancelTags;

        public string AbilityName => _AbilityName;
        public Sprite AbilityIcon => _AbilityIcon;
        public EAbilityType AbilityType => _AbilityType;
        public GameplayTag AbilityTag => _AbilityTag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => _AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => _CancelTags;

        [Header(" 디버그 ")]
        [SerializeField] private bool _DebugMode;

        public bool DebugMode => _DebugMode;
        public abstract AbilitySpec CreateSpec(AbilitySystem owner, int level);
    }
}
