#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{

    public abstract class GASAbilitySpec : AbilitySpec
    {
        protected new readonly GASAbility _Ability;
        protected readonly IGameplayTagSystem _gameplayTagSystem;


        protected GASAbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level) : base(ability, abilitySystem, level)
        {
            _Ability = ability as GASAbility;
            _gameplayTagSystem = abilitySystem.transform.GetComponent<IGameplayTagSystem>();
        }
        public override void CancelAbilityFromSource(object source)
        {
            if (source is not GameplayTag[] gameplayTags)
                return;

            foreach (var tag in gameplayTags)
            {
                if (_Ability.AbilityTag == tag || _Ability.AttributeTags.Contains(tag))
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

            if (_gameplayTagSystem.ContainBlockTag(_Ability.AbilityTag)
                || _gameplayTagSystem.ContainAnyTagsInBlock(_Ability.AttributeTags))
                return false;

            if (!_gameplayTagSystem.ContainConditionTags(_Ability.ConditionTags))
                return false;

            return true;
        }

        protected override void EnterAbility()
        {
            _AbilitySystem.CancelAbilityFromSource(_Ability.CancelAbilityTags);

            _gameplayTagSystem.GrantGameplayTags(_Ability.GrantTags);
        }

        protected override void ExitAbility()
        {
            _gameplayTagSystem.RemoveGameplayTags(_Ability.GrantTags);
        }
    }
}
#endif