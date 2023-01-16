using UnityEngine;
using System.Collections.Generic;

#if SCOR_ENABLE_GAMEPLAYTAG
using StudioScor.GameplayTagSystem;
#endif

namespace StudioScor.AbilitySystem
{
#if SCOR_ENABLE_GAMEPLAYTAG
    public abstract partial class Ability : ScriptableObject
    {
        private GameplayTag _Tag;
        private GameplayTag[] _AttributeTags;
        private GameplayTag[] _CancelTags;

        public GameplayTag Tag => _Tag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => _AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => _CancelTags;
    }
#endif
}
