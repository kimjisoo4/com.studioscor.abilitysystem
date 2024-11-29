#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;
using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{
    public abstract class GASAbilitySpec : AbilitySpec
    {
        protected new readonly GASAbility _ability;
        private readonly IGameplayTagSystem _gameplayTagSystem;
        protected IGameplayTagSystem GameplayTagSystem => _gameplayTagSystem;


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

            if (!CheckGameplayTags())
                return false;

            return true;
        }
        protected bool CheckGameplayTags()
        {
            if (_gameplayTagSystem.ContainBlockTag(_ability.AbilityTag)
               || _gameplayTagSystem.ContainAnyTagsInBlock(_ability.AttributeTags))
            {
                Log($"{nameof(CanActiveAbility)} - Has Bloking Tag", SUtility.STRING_COLOR_FAIL);
                return false;
            }

            if (!_gameplayTagSystem.ContainConditionTags(_ability.ConditionTags))
            {
                Log($"{nameof(CanActiveAbility)} - Not Contained Condition Tags", SUtility.STRING_COLOR_FAIL);
                return false;
            }

            return true;
        }
        protected override void EnterAbility()
        {
            _abilitySystem.CancelAbilityFromSource(_ability.CancelAbilityTags);

            _gameplayTagSystem.AddGameplayTags(_ability.GrantTags);
        }

        protected override void ExitAbility()
        {
            _gameplayTagSystem.RemoveGameplayTags(_ability.GrantTags);
        }
    }
}
#endif