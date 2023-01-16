using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#if SCOR_ENABLE_GAMEPLAYTAG
using StudioScor.GameplayTagSystem;
#endif

namespace StudioScor.AbilitySystem
{
    public abstract partial class AbilitySpec : IAbilitySpec
    {
#if SCOR_ENABLE_GAMEPLAYTAG
        public GameplayTagSystem.GameplayTagSystem GameplayTagComponent => AbilitySystem.GameplayTagComponent;
        public GameplayTag AbilityTag => Ability.Tag;
        public IReadOnlyCollection<GameplayTag> AttributeTags => Ability.AttributeTags;
        public IReadOnlyCollection<GameplayTag> CancelTags => Ability.CancelTags;

        public bool TryCancelAbility(GameplayTag[] cancelTags)
        {
            if (!IsPlaying)
                return false;

            if (cancelTags.Contains(Ability.Tag))
            {
                ForceCancelAbility();

                return true;
            }

            foreach (GameplayTag tag in cancelTags)
            {
                if (AttributeTags.Contains(tag))
                {
                    ForceCancelAbility();

                    return true;
                }
            }

            return false;
        }
#endif

        [Conditional("SCOR_ENABLE_GAMEPLAYTAG")]
        private void CancelAbilityWithCancelTags()
        {
#if SCOR_ENABLE_GAMEPLAYTAG
            AbilitySystem.CancelAbility(CancelTags);
#endif
        }
    }
}
