#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{

    public abstract class GASAbilitySpec : AbilitySpec
    {
        protected new readonly GASAbility ability;
        protected readonly IGameplayTagSystem gameplayTagSystem;


        protected GASAbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level) : base(ability, abilitySystem, level)
        {
            this.ability = ability as GASAbility;
            gameplayTagSystem = abilitySystem.transform.GetComponent<IGameplayTagSystem>();
        }
        public override void CancelAbilityFromSource(object source)
        {
            if (source is not GameplayTag[] gameplayTags)
                return;

            foreach (var tag in gameplayTags)
            {
                if (ability.AbilityTag == tag || ability.AttributeTags.Contains(tag))
                {
                    CancelAbility();

                    return;
                }
            }
        }

        public override bool CanActiveAbility()
        {
            if (!base.CanActiveAbility())
                return false;

            if (gameplayTagSystem.ContainBlockTag(ability.AbilityTag)
                || gameplayTagSystem.ContainAnyTagsInBlock(ability.AttributeTags))
                return false;

            if (!gameplayTagSystem.ContainConditionTags(ability.ConditionTags))
                return false;

            return true;
        }

        protected override void EnterAbility()
        {
            abilitySystem.CancelAbilityFromSource(ability.CancelAbilityTags);

            gameplayTagSystem.GrantGameplayTags(ability.GrantTags);
        }

        protected override void ExitAbility()
        {
            gameplayTagSystem.RemoveGameplayTags(ability.GrantTags);
        }
    }
}
#endif