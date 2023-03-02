#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using UnityEngine;
using System.Collections.Generic;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    public interface IGameplayTagTask
    {
        public void AddOwnedTags(IReadOnlyCollection<GameplayTag> addTags);
        public void AddBlockTags(IReadOnlyCollection<GameplayTag> addTags);
        public void RemoveOwnedTags(IReadOnlyCollection<GameplayTag> removeTags);
        public void RemoveBlockTags(IReadOnlyCollection<GameplayTag> removeTags);
        public bool ContainAllTagsInOwned(IReadOnlyCollection<GameplayTag> containTags);
        public bool ContainAnyTagsInBlock(IReadOnlyCollection<GameplayTag> containTags);
    }

    [CreateAssetMenu(menuName = "StudioScor/Ability/Task/new GameplayTagTask", fileName = "ATask_GameplayTag")]
    public class GameplayTagTask : Task
    {
        [Header(" [ GameplayTag Task ] ")]
        [SerializeField] private FConditionTags _ConditionTags;
        [SerializeField] private FGameplayTags _GrantTags;

        public FConditionTags ConditionTags => _ConditionTags;
        public FGameplayTags GrantTags => _GrantTags;

        public override ITaskSpec CreateSpec(GameObject owner)
        {
            return new Spec(this, owner);
        }

        public class Spec : AbilityTaskSpec<GameplayTagTask>
        {
            private readonly IGameplayTagTask _GameplayTagSystem;
            public override float Progress => IsPlaying ? 1f : 0f;

            public Spec(GameplayTagTask actionBlock, GameObject owner) : base(actionBlock, owner)
            {
                _GameplayTagSystem = owner.GetComponent<IGameplayTagTask>();
            }

            public override bool CanEnterTask()
            {
                return base.CanEnterTask() && CheckConditionTags();
            }

            private bool CheckConditionTags()
            {
                return _GameplayTagSystem.ContainAllTagsInOwned(AbilityTask.ConditionTags.Requireds)
                    && !_GameplayTagSystem.ContainAnyTagsInBlock(AbilityTask.ConditionTags.Obstacleds);
            }

            protected override void EnterTask()
            {
                Log(nameof(EnterTask));

                _GameplayTagSystem.AddOwnedTags(AbilityTask.GrantTags.Owneds);
                _GameplayTagSystem.AddBlockTags(AbilityTask.GrantTags.Blocks);
            }

            protected override void ExitTask()
            {
                Log(nameof(ExitTask));

                _GameplayTagSystem.RemoveOwnedTags(AbilityTask.GrantTags.Owneds);
                _GameplayTagSystem.RemoveBlockTags(AbilityTask.GrantTags.Blocks);
            }
        }
    }
}
#endif