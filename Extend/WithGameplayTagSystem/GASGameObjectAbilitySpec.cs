#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using System.Linq;
using StudioScor.GameplayTagSystem;
using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{
    public abstract class GASGameObjectAbilitySpec : GameObjectAbilitySpec
    {
        protected GASAbility _gasAbility;
        private IGameplayTagSystem _gameplayTagSystem;
        protected IGameplayTagSystem GameplayTagSystem => _gameplayTagSystem;

        public override void Setup(Ability ability, IAbilitySystem abilitySystem, int level = 0)
        {
            base.Setup(ability, abilitySystem, level);

            _gasAbility = ability as GASAbility;
            _gameplayTagSystem = abilitySystem.transform.GetComponent<IGameplayTagSystem>();
        }

        public override void CancelAbilityFromSource(object source)
        {
            if (source is not GameplayTag[] gameplayTags)
                return;

            foreach (var tag in gameplayTags)
            {
                if (_gasAbility.AbilityTag == tag || _gasAbility.AttributeTags.Contains(tag))
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
            if (_gameplayTagSystem.ContainBlockTag(_gasAbility.AbilityTag)
               || _gameplayTagSystem.ContainAnyTagsInBlock(_gasAbility.AttributeTags))
            {
                Log($"{nameof(CanActiveAbility)} - Has Bloking Tag", SUtility.STRING_COLOR_FAIL);
                return false;
            }

            if (!_gameplayTagSystem.ContainConditionTags(_gasAbility.ConditionTags))
            {
                Log($"{nameof(CanActiveAbility)} - Not Contained Condition Tags", SUtility.STRING_COLOR_FAIL);
                return false;
            }

            return true;
        }
        protected override void EnterAbility()
        {
            _abilitySystem.CancelAbilityFromSource(_gasAbility.CancelAbilityTags);

            _gameplayTagSystem.AddGameplayTags(_gasAbility.GrantTags);
        }

        protected override void ExitAbility()
        {
            _gameplayTagSystem.RemoveGameplayTags(_gasAbility.GrantTags);
        }
    }
}
#endif