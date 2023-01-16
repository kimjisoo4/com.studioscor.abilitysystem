using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;
using System;

using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract partial class AbilitySpecWithMono : MonoBehaviour, IAbilitySpec
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
