#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    [System.Serializable]
    public struct FAbilityTags
    {
        public GameplayTagSO[] AttributeTags;
        public GameplayTagSO[] RequiredTags;
        public GameplayTagSO[] ObstacledTags;
    }
}
#endif