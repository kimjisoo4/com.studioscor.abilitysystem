#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{

    public abstract class GASAbilitySpec : AbilitySpec
    {
        protected new readonly GASAbility _Ability;
        private readonly IGameplayTagSystem _GameplayTagSystem;

        protected IGameplayTagSystem GameplayTagSystem => _GameplayTagSystem;


        protected GASAbilitySpec(Ability ability, IAbilitySystem abilitySystem, int level) : base(ability, abilitySystem, level)
        {
            _Ability = ability as GASAbility;
            _GameplayTagSystem = abilitySystem.transform.GetComponent<IGameplayTagSystem>();
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

            if (_GameplayTagSystem.ContainBlockTag(_Ability.AbilityTag)
                || _GameplayTagSystem.ContainAnyTagsInBlock(_Ability.AttributeTags))
                return false;

            if (!_GameplayTagSystem.ContainConditionTags(_Ability.ConditionTags))
                return false;

            return true;
        }

        protected override void EnterAbility()
        {
            _AbilitySystem.CancelAbilityFromSource(_Ability.CancelAbilityTags);

            _GameplayTagSystem.GrantGameplayTags(_Ability.GrantTags);
        }

        protected override void ExitAbility()
        {
            _GameplayTagSystem.RemoveGameplayTags(_Ability.GrantTags);
        }
    }
}
#endif