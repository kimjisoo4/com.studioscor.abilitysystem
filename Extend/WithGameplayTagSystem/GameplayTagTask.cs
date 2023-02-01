#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using UnityEngine;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem.GameplayTagSystem
{
    [System.Serializable]
    public class GameplayTagTask : AbilityTask
    {
        [Header(" [ GameplayTag Task ] ")]
        [SerializeField] private FConditionTags _ConditionTags;
        [SerializeField] private FGameplayTags _GrantTags;

        public FConditionTags ConditionTags => _ConditionTags;
        public FGameplayTags GrantTags => _GrantTags;

        public override IAbilityTaskSpec CreateSpec(IAbilitySpec abilitySpec)
        {
            return new Spec(this, abilitySpec);
        }

        public class Spec : AbilityTaskSpec<GameplayTagTask>
        {
            private readonly GameplayTagSystemComponent _GameplayTagSystemComponent;

            public Spec(GameplayTagTask actionBlock, IAbilitySpec abilitySpec) : base(actionBlock, abilitySpec)
            {
                _GameplayTagSystemComponent = abilitySpec.AbilitySystemComponent.GetComponent<GameplayTagSystemComponent>();
            }

            public override bool CanEnterTask()
            {
                return base.CanEnterTask() && CheckConditionTags();
            }

            private bool CheckConditionTags()
            {
                return _GameplayTagSystemComponent.ContainAllTagsInOwned(AbilityTask.ConditionTags.Requireds)
                    && !_GameplayTagSystemComponent.ContainAnyTagsInBlock(AbilityTask.ConditionTags.Obstacleds);
            }

            protected override void EnterTask()
            {
                Log(nameof(EnterTask));

                _GameplayTagSystemComponent.AddOwnedTags(AbilityTask.GrantTags.Owneds);
                _GameplayTagSystemComponent.AddBlockTags(AbilityTask.GrantTags.Blocks);
            }

            protected override void ExitTask()
            {
                Log(nameof(ExitTask));

                _GameplayTagSystemComponent.RemoveOwnedTags(AbilityTask.GrantTags.Owneds);
                _GameplayTagSystemComponent.RemoveBlockTags(AbilityTask.GrantTags.Blocks);
            }
        }
    }
}
#endif