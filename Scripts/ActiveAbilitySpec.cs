using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace KimScor.GameplayTagSystem.Ability
{
    public class TaskAbility : ActiveAbility
    {
        public override AbilitySpec CreateSpec(AbilitySystem owner, int level)
        {
            var spec = new TaskAbilitySpec(this, owner,level);

            return spec;
        }
        public class TaskAbilitySpec : ActiveAbilitySpec
        {
            public TaskAbilitySpec(Ability ability, AbilitySystem owner, int level) : base(ability, owner, level)
            {
            }

            public override void OnGrantAbility()
            {
                base.OnGrantAbility();
            }
            public override void OnLostAbility()
            {
            }
            protected override void EnterAbility()
            {
            }

            protected override void ExitAbility()
            {
            }

            protected override void CancelAbility()
            {
            }

            

            protected override void ReleasedAbility()
            {
            }

            protected override void ReTriggerAbility()
            {
            }
        }
    }
    public abstract class ActiveAbilitySpec : AbilitySpec
    {
        public ActiveAbilitySpec(Ability ability, AbilitySystem owner, int level) : base(ability, owner, level)
        {
            _ActiveAbility = ability as ActiveAbility;
        }

        private ActiveAbility _ActiveAbility;
        public ActiveAbility ActiveAbility => _ActiveAbility;
        public FAbilityTags AbilityTags => ActiveAbility.AbilityTags;

        public override void OnGrantAbility()
        {
            Owner.OnCanceledAbility += Owner_OnCanceledAbility;
        }

        private void Owner_OnCanceledAbility(AbilitySystem abilitySystem, GameplayTag[] cancelTags)
        {
            if (!Activate)
                return;

            if (cancelTags.Contains(Ability.AbilityTag))
            {
                CancelAbility();

                return;
            }

            foreach (GameplayTag tag in cancelTags)
            {
                if (AttributeTags.Contains(tag))
                {
                    CancelAbility();

                    return;
                }
            }
        }

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
            if (GameplayTagSystem.ContainBlockTag(Ability.AbilityTag))
            {
                if (Ability.DebugMode)
                    Debug.Log("블록되어 있는 어빌리티");

                return false;
            }

            if (!GameplayTagSystem.ContainNotOnceBlockTags(Ability.AttributeTags.ToArray()))
            {
                if (Ability.DebugMode)
                    Debug.Log("블록되어 있는 태그 속성");

                return false;
            }

            // 해당 태그가 모두 존재하고 있는가
            if (!GameplayTagSystem.ContainOnceOwnedTags(AbilityTags.ActivationRequiredTags))
            {
                if (Ability.DebugMode)
                    Debug.Log("필수 태그 없음");

                return false;
            }

            // 해당 태그가 하나라도 존재하고 있는가
            if (!GameplayTagSystem.ContainNotOnceOwnedTags(AbilityTags.ActivationBlockedTags))
            {
                if (Ability.DebugMode)
                    Debug.Log("없어야할 태그 존재");

                return false;
            }

            return true;
        }
    }



}
