using UnityEngine;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

#if SCOR_ENABLE_GAMEPLAYTAG
using StudioScor.GameplayTagSystem;
#endif

namespace StudioScor.AbilitySystem
{
    public partial class AbilitySystem : MonoBehaviour
    {

#if SCOR_ENABLE_GAMEPLAYTAG
        [Header(" [ Use GameplayTagSystem ] ")]
        [SerializeField] private GameplayTagSystem.GameplayTagSystem _GameplayTagComponent;

        public GameplayTagSystem.GameplayTagSystem GameplayTagComponent
        {
            get
            {
                if (!_WasSetup)
                    Setup();

                return _GameplayTagComponent;
            }
        }
#endif

        [Conditional("SCOR_ENABLE_GAMEPLAYTAG")]
        protected void SetupGameplayTag()
        {
#if SCOR_ENABLE_GAMEPLAYTAG
            if (!_GameplayTagComponent)
            {
                if (!TryGetComponent(out _GameplayTagComponent))
                {
                    Log("GameplayTag System Is Null", true);
                }
            }
#endif
        }

#if SCOR_ENABLE_GAMEPLAYTAG
        public bool TryGetAbilitySpec(GameplayTag abilityTag, out IAbilitySpec abilitySpec)
        {
            abilitySpec = GetAbilitySpec(abilityTag);

            return abilitySpec is not null;
        }

        public IAbilitySpec GetAbilitySpec(GameplayTag abilityTag)
        {
            foreach (IAbilitySpec spec in Abilities)
            {
                if (spec.Ability.Tag == abilityTag)
                {
                    return spec;
                }
            }

            return null;
        }
        public bool HasAbility(GameplayTag abilityTag)
        {
            return GetAbilitySpec(abilityTag) is not null;
        }
        public bool IsPlayingAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                return spec.IsPlaying;
            }
            else
            {
                return false;
            }
        }
        public bool TryActivateAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                return spec.TryActiveAbility();
            }

            return false;
        }
        public void OnReleasedAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                if (spec.IsPlaying)
                    spec.OnReleaseAbility();
            }
        }
        public void CancelAbility(GameplayTag[] cancelTags)
        {
            foreach (IAbilitySpec spec in Abilities)
            {
                spec.TryCancelAbility(cancelTags);
            }
        }
        public void CancelAbility(IReadOnlyCollection<GameplayTag> cancelTags)
        {
            GameplayTag[] tags = cancelTags.ToArray();

            foreach (IAbilitySpec spec in Abilities)
            {
                spec.TryCancelAbility(tags);
            }
        }
        
        public bool CanActiveAbility(GameplayTag abilityTag)
        {
            if (TryGetAbilitySpec(abilityTag, out IAbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
#endif
    }

}
