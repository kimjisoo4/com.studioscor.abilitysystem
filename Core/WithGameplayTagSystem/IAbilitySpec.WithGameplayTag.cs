#if SCOR_ENABLE_GAMEPLAYTAG
using StudioScor.GameplayTagSystem;
#endif

namespace StudioScor.AbilitySystem
{
    public partial interface IAbilitySpec
    {
#if SCOR_ENABLE_GAMEPLAYTAG
        public bool TryCancelAbility(GameplayTag[] cancelTags);
#endif
    }
}
