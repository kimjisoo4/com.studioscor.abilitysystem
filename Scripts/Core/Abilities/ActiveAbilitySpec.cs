using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class ActiveAbilitySpec : AbilitySpec
    {
        public ActiveAbilitySpec(Ability ability, AbilitySystem owner, int level) : base(ability, owner, level)
        {
            _ActiveAbility = ability as ActiveAbility;
        }

        private ActiveAbility _ActiveAbility;
        public ActiveAbility ActiveAbility => _ActiveAbility;
        public FAbilityTags AbilityTags => ActiveAbility.AbilityTags;


        public override void OnAbility()
        {
            Owner.OnCancelAbility(AbilityTags.CancelAbilitiesWithTag);

            GameplayTagSystem.AddOwnedTags(AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.AddBlockTags(AbilityTags.BlockAbilitiesWithTag);

            base.OnAbility();
        }

        public override void EndAbility()
        {
            base.EndAbility();

            GameplayTagSystem.RemoveOwnedTags(AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.RemoveBlockTags(AbilityTags.BlockAbilitiesWithTag);
        }

        public override void OnCancelAbility()
        {
            base.OnCancelAbility();

            GameplayTagSystem.RemoveOwnedTags(AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.RemoveBlockTags(AbilityTags.BlockAbilitiesWithTag);
        }

        public override bool CanActiveAbility()
        {
            // 어빌리티 태그가 블록 되어 있는가
            if (Ability.AbilityTag is not null
                && GameplayTagSystem.ContainBlockTag(Ability.AbilityTag))
            {
                Log("어빌리티가 블록되어 있음.");

                return false;
            }

            // 속성 태그 중에 블록되어 있는게 있는가
            if (Ability.AttributeTags is not null 
                && GameplayTagSystem.ContainOnceTagsInBlock(Ability.AttributeTags.ToArray()))
            {
                Log("어빌리티의 속성이 블록되어 있음.");

                return false;
            }

            // 해당 태그가 모두 존재하고 있는가
            if (AbilityTags.ActivationRequiredTags is not null 
                && !GameplayTagSystem.ContainAllTagsInOwned(AbilityTags.ActivationRequiredTags))
            {
                Log("필수 태그를 소유하고 있지 않음");

                return false;
            }

            // 해당 태그를 가지고 있는가
            if (AbilityTags.ActivationBlockedTags is not null
                && GameplayTagSystem.ContainOnceTagsInOwned(AbilityTags.ActivationBlockedTags))
            {
                Log("방해 태그를 소유하고 있음");

                return false;
            }

            return true;
        }
    }



}
