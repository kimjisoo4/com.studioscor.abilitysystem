#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public abstract class GASAbilitySpec : AbilitySpec
    {
        private readonly IGASAbility _GASAbility;
        private readonly IGameplayTagSystem _GameplayTagSystem;

        public IGASAbility GASAbility => _GASAbility;
        public IGameplayTagSystem GameplayTagSystem => _GameplayTagSystem;


        protected GASAbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level) : base(ability, abilitySystem, level)
        {
            _GASAbility = ability as IGASAbility;
            _GameplayTagSystem = abilitySystem.transform.GetComponent<IGameplayTagSystem>();
        }
        public override void CancelAbilityFromSource(object source)
        {
            if (source is not GameplayTag[] gameplayTags)
                return;

            foreach (var tag in gameplayTags)
            {
                if (GASAbility.AbilityTag == tag || GASAbility.AttributeTags.Contains(tag))
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

            if (_GameplayTagSystem.ContainBlockTag(GASAbility.AbilityTag)
                || _GameplayTagSystem.ContainAnyTagsInBlock(GASAbility.AttributeTags))
                return false;

            if (!_GameplayTagSystem.ContainAllTagsInOwned(GASAbility.ConditionTags.Requireds)
                || _GameplayTagSystem.ContainAnyTagsInOwned(GASAbility.ConditionTags.Obstacleds))
                return false;

            return true;
        }

        protected override void EnterAbility()
        {
            _AbilitySystem.CancelAbilityFromSource(GASAbility.CancelAbilityTags);

            _GameplayTagSystem.AddOwnedTags(GASAbility.GrantTags.Owneds);
            _GameplayTagSystem.AddBlockTags(GASAbility.GrantTags.Blocks);
        }

        protected override void ExitAbility()
        {
            _GameplayTagSystem.RemoveOwnedTags(GASAbility.GrantTags.Owneds);
            _GameplayTagSystem.RemoveBlockTags(GASAbility.GrantTags.Blocks);
        }
    }
}
#endif