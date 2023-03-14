#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using UnityEngine;
using StudioScor.GameplayTagSystem;

namespace StudioScor.AbilitySystem
{
    [CreateAssetMenu(menuName = "StudioScor/TaskSystem/new GameplayTagTask", fileName = "Task_GameplayTag")]
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

        public class Spec : TaskSpec
        {
            private new readonly GameplayTagTask _Task;
            private readonly IGameplayTagSystem _GameplayTagSystem;
            public override float Progress => IsPlaying ? 1f : 0f;

            public Spec(Task task, GameObject owner) : base(task, owner)
            {
                _Task = task as GameplayTagTask;
                _GameplayTagSystem = owner.GetComponent<IGameplayTagSystem>();
            }

            public override bool CanEnterTask()
            {
                return base.CanEnterTask() && CheckConditionTags();
            }

            private bool CheckConditionTags()
            {
                return _GameplayTagSystem.ContainAllTagsInOwned(_Task.ConditionTags.Requireds)
                    && !_GameplayTagSystem.ContainAnyTagsInBlock(_Task.ConditionTags.Obstacleds);
            }

            protected override void EnterTask()
            {
                Log(nameof(EnterTask));

                _GameplayTagSystem.AddOwnedTags(_Task.GrantTags.Owneds);
                _GameplayTagSystem.AddBlockTags(_Task.GrantTags.Blocks);
            }

            protected override void ExitTask()
            {
                Log(nameof(ExitTask));

                _GameplayTagSystem.RemoveOwnedTags(_Task.GrantTags.Owneds);
                _GameplayTagSystem.RemoveBlockTags(_Task.GrantTags.Blocks);
            }
        }
    }
}
#endif