using System.Diagnostics;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public partial class AbilityStateBlockBaseSpec : AbilityStateBlockSpec
    {
#if SCOR_ENABLE_GAMEPLAYTAG
        protected GameplayTagSystem.GameplayTagSystem _GameplayTagComponent;
#endif
        [Conditional("SCOR_ENABLE_GAMEPLAYTAG")]
        private void SetGameplayTag()
        {
#if SCOR_ENABLE_GAMEPLAYTAG
            _GameplayTagComponent = _AbilityState.AbilitySpec.AbilitySystem.GameplayTagComponent;
#endif
        }

        private bool HasRequiredTags()
        {
#if SCOR_ENABLE_GAMEPLAYTAG
            return _AbilityStateBlock.ConditionTags.Requireds is null
                || _GameplayTagComponent.ContainAllTagsInOwned(_AbilityStateBlock.ConditionTags.Requireds);
#else
            return true;
#endif

        }
        private bool HasObstacledTag()
        {
#if SCOR_ENABLE_GAMEPLAYTAG
            return _AbilityStateBlock.ConditionTags.Obstacleds is not null
                && _GameplayTagComponent.ContainAnyTagsInOwned(_AbilityStateBlock.ConditionTags.Obstacleds);
#else
            return false;
#endif
        }
        [Conditional("SCOR_ENABLE_GAMEPLAYTAG")]
        private void AddTags()
        {
#if SCOR_ENABLE_GAMEPLAYTAG
            _GameplayTagComponent.AddOwnedTags(_AbilityStateBlock.GrantTags.Owneds);
            _GameplayTagComponent.AddBlockTags(_AbilityStateBlock.GrantTags.Blocks);
#endif
        }
        [Conditional("SCOR_ENABLE_GAMEPLAYTAG")]
        private void RemoveTags()
        {
#if SCOR_ENABLE_GAMEPLAYTAG
            _GameplayTagComponent.RemoveOwnedTags(_AbilityStateBlock.GrantTags.Owneds);
            _GameplayTagComponent.RemoveBlockTags(_AbilityStateBlock.GrantTags.Blocks);
#endif
        }

    }
    public partial class AbilityStateBlockBaseSpec : AbilityStateBlockSpec
    {
        protected readonly AbilityStateBlock _AbilityStateBlock;
        protected readonly IAbilityState _AbilityState;

        public override bool UseDebug => _AbilityStateBlock is not null && _AbilityStateBlock.UseDebug;
        protected virtual bool CanCancelState => true;

        public AbilityStateBlockBaseSpec(AbilityStateBlock abilityStateBlock, IAbilityState abilityState)
        {
            _AbilityStateBlock = abilityStateBlock;
            _AbilityState = abilityState;

            SetGameplayTag();
        }

        protected override void Log(object log, bool isError = false, UnityEngine.Object context = null)
        {
            base.Log(log, isError, _AbilityStateBlock);
        }

        public override bool CanEnterState()
        {
            if (!base.CanEnterState())
                return false;

            if (!HasRequiredTags())
                return false;

            if (HasObstacledTag())
                return false;
            
            return true;
        }
        public override bool CanExitState()
        {
            if (!base.CanExitState())
                return false;

            if (!CanCancelState)
                return false;

            return true;
        }

        protected override void EnterState()
        {
            AddTags();
        }

        protected override void ExitState()
        {
            RemoveTags();
        }

    }
}
