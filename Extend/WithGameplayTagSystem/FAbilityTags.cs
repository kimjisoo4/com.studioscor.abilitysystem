#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem.VisualScripting.GameplayTagSystem
{
    [System.Serializable]
    public struct FAbilityTags
    {
        public GameplayTag[] AttributeTags;
        public GameplayTag[] RequiredTags;
        public GameplayTag[] ObstacledTags;
    }
}
#endif