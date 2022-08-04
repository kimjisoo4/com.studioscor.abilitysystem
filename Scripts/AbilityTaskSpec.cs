using System.Collections.Generic;
using UnityEngine;

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class AbilityTaskSpec
    {
        private AbilityTask _AbilityTask;
        private GameplayTagSystem _GameplayTagSystem;
        private TaskAbilitySpec _TaskAbilitySpec;
        public AbilityTask AbilityTask => _AbilityTask;
        public GameplayTagSystem GameplayTagSystem => _GameplayTagSystem;

        public TaskAbilitySpec TaskAbilitySpec => _TaskAbilitySpec;

        private bool _IsActive = false;
        public bool IsActive => _IsActive;
        public GameplayTag ActiveTag => AbilityTask.ActiveTag;
        public IReadOnlyCollection<GameplayTag> StartedTags => AbilityTask.StartedTags;
        public IReadOnlyCollection<GameplayTag> FinishedTags => AbilityTask.FinishedTags;

        public AbilityTaskSpec(AbilityTask abilityTask, GameplayTagSystem gameplayTagSysetm, TaskAbilitySpec taskAbilitySpec)
        {
            _AbilityTask = abilityTask;
            _GameplayTagSystem = gameplayTagSysetm;
            _TaskAbilitySpec = taskAbilitySpec;

            SetupGameplayTag();
        }
        private void SetupGameplayTag()
        {
            if (ActiveTag is not null)
            {

                Debug.Log("Chjeck");
                _GameplayTagSystem.OnTriggerTag += GameplayTagSystem_OnTriggerTag;
            }
        }

        private void GameplayTagSystem_OnTriggerTag(GameplayTagSystem gameplayTagSystem, GameplayTag changedTag)
        {
            if (changedTag == ActiveTag)
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

            GameplayTagSystem.TriggerTags(StartedTags);

            EnterTask();
        }
        public void EndTask()
        {
            if (!_IsActive)
                return;

            _IsActive = false;

            GameplayTagSystem.TriggerTags(FinishedTags);

            ExitTask();
        }
        public virtual void DestroyTask()
        {
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