#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{

    public abstract class GASAbilitySpec : AbilitySpec
    {
        protected new readonly GASAbility _ability;
        protected readonly IGameplayTagSystem _gameplayTagSystem;


        protected GASAbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level) : base(ability, abilitySystem, level)
        {
            _ability = ability as GASAbility;
            _gameplayTagSystem = abilitySystem.transform.GetComponent<IGameplayTagSystem>();
        }
        public override void CancelAbilityFromSource(object source)
        {
            if (source is not GameplayTag[] gameplayTags)
                return;

            foreach (var tag in gameplayTags)
            {
                if (_ability.AbilityTag == tag || _ability.AttributeTags.Contains(tag))
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

            if (_gameplayTagSystem.ContainBlockTag(_ability.AbilityTag)
                || _gameplayTagSystem.ContainAnyTagsInBlock(_ability.AttributeTags))
                return false;

            if (!_gameplayTagSystem.ContainConditionTags(_ability.ConditionTags))
                return false;

            return true;
        }

        protected override void EnterAbility()
        {
            _abilitySystem.CancelAbilityFromSource(_ability.CancelAbilityTags);

            _gameplayTagSystem.GrantGameplayTags(_ability.GrantTags);
        }

        protected override void ExitAbility()
        {
            _gameplayTagSystem.RemoveGameplayTags(_ability.GrantTags);
        }
    }
}
#endif