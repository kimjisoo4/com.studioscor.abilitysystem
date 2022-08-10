using System.Collections.Generic;

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class AbilityTaskSpec
    {
        private AbilityTask _AbilityTask;
        private TaskAbilitySpec _TaskAbilitySpec;
        public AbilityTask AbilityTask => _AbilityTask;
        public TaskAbilitySpec TaskAbilitySpec => _TaskAbilitySpec;

        private bool _IsActive = false;
        public bool IsActive => _IsActive;
        public AbilityTaskTag ActiveTag => AbilityTask.ActiveTag;
        public IReadOnlyCollection<AbilityTaskTag> StartedTags => AbilityTask.StartedTags;
        public IReadOnlyCollection<AbilityTaskTag> FinishedTags => AbilityTask.FinishedTags;

        public AbilityTaskSpec(AbilityTask abilityTask, TaskAbilitySpec taskAbilitySpec)
        {
            _AbilityTask = abilityTask;
            _TaskAbilitySpec = taskAbilitySpec;

            SetupGameplayTag();
        }
        private void SetupGameplayTag()
        {
            if (ActiveTag is not null)
            {
                TaskAbilitySpec.OnTriggeredTag += TaskAbilitySpec_OnTriggeredTag;
            }
        }

        private void TaskAbilitySpec_OnTriggeredTag(TaskAbilitySpec taskAbilitySpec, AbilityTaskTag triggerTag)
        {
            if (triggerTag == ActiveTag)
            {
                if (IsActive)
                {
                    EndTask();
                }
                else
                {
                    OnTask();
                }
            }
        }

        public void OnTask()
        {
            if (_IsActive)
                return;

            _IsActive = true;

            if(StartedTags is not null)
                TaskAbilitySpec.OnTriggerTags(StartedTags);

            EnterTask();
        }
        public void EndTask()
        {
            if (!_IsActive)
                return;

            _IsActive = false;

            if (FinishedTags is not null)
                TaskAbilitySpec.OnTriggerTags(FinishedTags);

            ExitTask();
        }
        public virtual void DestroyTask()
        {
            if (StartedTags is not null)
            {
                TaskAbilitySpec.OnTriggeredTag -= TaskAbilitySpec_OnTriggeredTag;
            }

            return;
        }

        protected virtual void EnterTask()
        {
            return;
        }
        public virtual void UpdateTask(float deltaTime)
        {
            return;
        }
        protected virtual void ExitTask()
        {
            return;
        }
    }
}