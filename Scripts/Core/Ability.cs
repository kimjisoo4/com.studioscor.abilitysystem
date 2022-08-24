using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class Ability : ScriptableObject
    {
#if ODIN_INSPECTOR
        [VerticalGroup("Ability/Properties")]
        [PropertyOrder(1)]
#else
        [Header(" 어빌리티의 이름 ")]
#endif
        [SerializeField] protected string _AbilityName;

#if ODIN_INSPECTOR
        [PropertyOrder(0)]
        [HorizontalGroup("Ability", Width = 50), HideLabel, PreviewField(50)]
#else
        [Header(" 어빌리티의 아이콘")]
#endif
        [SerializeField] protected Sprite _AbilityIcon;

#if ODIN_INSPECTOR
        [VerticalGroup("Ability/Properties")]
        [PropertyOrder(3)]
        [EnumToggleButtons]
#else
        [Header(" 어빌리티의 종류")]
#endif
        [SerializeField] protected EAbilityType _AbilityType;

#if ODIN_INSPECTOR
        [BoxGroup("Gameplay Tags")]
        [PropertyOrder(0)]
#else
        [Header(" 어빌리티의 태그 ")]
#endif
        [SerializeField] private GameplayTag _AbilityTag;

#if ODIN_INSPECTOR
        [BoxGroup("Gameplay Tags")]
        [PropertyOrder(1)]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header(" 어빌리티의 속성 ")]
#endif
        [SerializeField] private GameplayTag[] _AttributeTags;

#if ODIN_INSPECTOR
        [BoxGroup("Gameplay Tags")]
        [PropertyOrder(2)]
        [ListDrawerSettings(Expanded = true)]
#else
        [Header(" 어빌리티 취소")]
#endif
        [SerializeField] private GameplayTag[] _CancelTags;

        public string AbilityName => _AbilityName;
        public Sprite AbilityIcon => _AbilityIcon;
        public EAbilityType AbilityType => _AbilityType;
        public GameplayTag AbilityTag => _AbilityTag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => _AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => _CancelTags;

        [Header(" 디버그 ")]
        [SerializeField] private bool _DebugMode;

        public bool UseDebugMode => _DebugMode;
        public abstract AbilitySpec CreateSpec(AbilitySystem owner, int level);
    }
}
