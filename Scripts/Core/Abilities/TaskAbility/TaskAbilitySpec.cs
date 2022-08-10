using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KimScor.GameplayTagSystem.Ability
{

    public class TaskAbilitySpec : AbilitySpec
    {
        #region Events
        public delegate void TriggerHandler(TaskAbilitySpec taskAbilitySpec, AbilityTaskTag triggerTag);
        #endregion

        private TaskAbility _TaskAbility;

        protected FAbilityTaskSpecContainer[] _AbilityTaskSpecs;
        public IReadOnlyCollection<FAbilityTaskSpecContainer> AbilitySpecContainers => _AbilityTaskSpecs;
        public virtual IReadOnlyCollection<FAbilityTaskSpecContainer> CurrentAbilitySpecContainers => AbilitySpecContainers;
        public FAbilityTaskSpecContainer PrevSpec => CurrentAbilitySpecContainers.ElementAt(_CurrentNumber - 1);
        public FAbilityTaskSpecContainer CurrentSpec => CurrentAbilitySpecContainers.ElementAt(_CurrentNumber);
        public FAbilityTaskSpecContainer NextSpec => CurrentAbilitySpecContainers.ElementAt(_CurrentNumber + 1);

        private int _CurrentNumber = 0;
        public int CurrentNumber => _CurrentNumber;
        public bool CanNextSpec => CurrentNumber < CurrentAbilitySpecContainers.Count - 1;

        public event TriggerHandler OnTriggeredTag;


        private bool _CanNextTask= false;
        public bool CanNextTask => _CanNextTask;


        public TaskAbilitySpec(Ability ability, AbilitySystem owner, int level) : base(ability, owner, level)
        {
            _TaskAbility = ability as TaskAbility;
        }

        #region Trigger Tag
        public void OnTriggerTag(AbilityTaskTag gameplayTag)
        {
            if (gameplayTag is null)
                return;

            OnTriggeredTag?.Invoke(this, gameplayTag);

            TriggerTag(gameplayTag);
        }
        public void OnTriggerTags(AbilityTaskTag[] gameplayTags)
        {
            if (gameplayTags.Length <= 0)
                return;

            foreach (var tag in gameplayTags)
            {
                OnTriggerTag(tag);
            }
        }
        public void OnTriggerTags(IReadOnlyCollection<AbilityTaskTag> gameplayTags)
        {
            if (gameplayTags.Count <= 0)
                return;

            foreach (var tag in gameplayTags)
            {
                OnTriggerTag(tag);
            }
        }
        private void TriggerTag(AbilityTaskTag changedTag)
        {
            if (CurrentSpec.ReTriggerTag && changedTag == CurrentSpec.ReTriggerTag)
            {
                TryReTriggerAbility();
            }
            else if (CurrentSpec.EndAbilityTag && changedTag == CurrentSpec.EndAbilityTag)
            {
                EndAbility();
            }
            else if (CurrentSpec.CanNextTaskTag && changedTag == CurrentSpec.CanNextTaskTag)
            {
                _CanNextTask = !_CanNextTask;
            }
        }
        #endregion

        public override void OnGrantAbility()
        {
            OnSetupAbilitySpec(ref _AbilityTaskSpecs, _TaskAbility.AbilityTasks.ToArray());
        }

        protected AbilityTaskSpec[] CreateTaskSpec(IReadOnlyCollection<AbilityTask> tasks)
        {
            int count = tasks.Count;
            var specs = new AbilityTaskSpec[count];

            for(int i = 0; i < count; i++)
            {
                specs[i] = tasks.ElementAt(i).CreateSpec(this);
            }

            return specs;
        }

        protected void OnSetupAbilitySpec(ref FAbilityTaskSpecContainer[] abilityTaskSpecContainer, FAbilityTaskContainer[] abilityTaskContainers)
        {
            if (abilityTaskContainers.Length <= 0)
            {
                return;
            }

            abilityTaskSpecContainer = new FAbilityTaskSpecContainer[abilityTaskContainers.Length];

            for (int i = 0; i < abilityTaskSpecContainer.Length; i++)
            {
                abilityTaskSpecContainer[i].ActivateTag = abilityTaskContainers.ElementAt(i).ActivateTag;
                abilityTaskSpecContainer[i].EndAbilityTag = abilityTaskContainers.ElementAt(i).EndAbilityTag;
                abilityTaskSpecContainer[i].ReTriggerTag = abilityTaskContainers.ElementAt(i).ReTriggerTag;
                abilityTaskSpecContainer[i].AbilityTags = abilityTaskContainers.ElementAt(i).AbilityTags;

                abilityTaskSpecContainer[i].CanNextTaskTag = abilityTaskContainers.ElementAt(i).CanNextTaskTag;
                
                if (abilityTaskContainers.ElementAt(i).AbilityCost is not null)
                {
                    abilityTaskSpecContainer[i].AbilityCost = abilityTaskContainers.ElementAt(i).AbilityCost.CreateSpec(this);
                }
                
                abilityTaskSpecContainer[i].AbilityTaskSpecs = CreateTaskSpec(abilityTaskContainers.ElementAt(i).AbilityTasks);
            }
        }



        public override void OnLostAbility()
        {
            foreach (FAbilityTaskSpecContainer specContainers in _AbilityTaskSpecs)
            {
                for (int i = specContainers.AbilityTaskSpecs.Length; i >= 0; i--)
                {
                    specContainers.AbilityTaskSpecs[i].DestroyTask();
                }
            }

            _AbilityTaskSpecs = null;
        }

        protected override void CancelAbility()
        {
            GameplayTagSystem.RemoveOwnedTags(CurrentSpec.AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.RemoveBlockTags(CurrentSpec.AbilityTags.ActivationBlockedTags);

            _CurrentNumber = 0;
            _CanNextTask = false;

            Owner.OnFixedUpdatedAbility -= Owner_OnFixedUpdatedAbility;
        }

        protected override void EnterAbility()
        {
            PlayAbility();

            Owner.OnFixedUpdatedAbility += Owner_OnFixedUpdatedAbility;
        }

        private void Owner_OnFixedUpdatedAbility(AbilitySystem abilitySystem, float deltaTime)
        {
            foreach (AbilityTaskSpec abilitySpec in CurrentSpec.AbilityTaskSpecs)
            {
                abilitySpec.UpdateTask(deltaTime);
            }
        }

        protected override void ExitAbility()
        {
            GameplayTagSystem.RemoveOwnedTags(CurrentSpec.AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.RemoveBlockTags(CurrentSpec.AbilityTags.ActivationBlockedTags);

            _CurrentNumber = 0;

            Owner.OnFixedUpdatedAbility -= Owner_OnFixedUpdatedAbility;
        }

        protected override void ReleasedAbility()
        {

        }

        protected override void ReTriggerAbility()
        {
            _CurrentNumber++;

            foreach (AbilityTaskSpec abilitySpec in PrevSpec.AbilityTaskSpecs)
            {
                abilitySpec.EndTask();
            }

            GameplayTagSystem.RemoveOwnedTags(PrevSpec.AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.RemoveBlockTags(PrevSpec.AbilityTags.ActivationBlockedTags);

            _CanNextTask = false;

            PlayAbility();
        }
        private void PlayAbility()
        {
            if (CurrentSpec.AbilityCost is not null)
            {
                CurrentSpec.AbilityCost.ConsumeCost();
            }

            GameplayTagSystem.AddOwnedTags(CurrentSpec.AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.AddBlockTags(CurrentSpec.AbilityTags.ActivationBlockedTags);

            Owner.OnCancelAbility(CurrentSpec.AbilityTags.CancelAbilitiesWithTag);

            OnTriggerTag(CurrentSpec.ActivateTag);
        }

        protected bool CheckAbilityTags(FAbilityTags abilityTags)
        {
            if (!GameplayTagSystem.ContainAllOwnedTags(abilityTags.ActivationRequiredTags))
            {
                return false;
            }
            if (!GameplayTagSystem.ContainNotAllBlockTags(abilityTags.ActivationBlockedTags))
            {
                return false;
            }
            return true;
        }

        public override bool CanActiveAbility()
        {
            if (GameplayTagSystem.ContainBlockTag(AbilityTag))
            {
                return false;
            }
            if (!GameplayTagSystem.ContainNotAllBlockTags(AttributeTags.ToArray()))
            {
                return false;
            }
            if (!CheckAbilityTags(CurrentSpec.AbilityTags))
            {
                return false;
            }
            if (CurrentSpec.AbilityCost is not null && !CurrentSpec.AbilityCost.CanConsumeCost())
            {
                return false;
            }

            return true;
        }
        public override bool CanReTriggerAbility()
        {
            if (!CanNextSpec)
            {
                return false;
            }
            if (!CanNextTask)
            {
                return false;
            }
            if (!CheckAbilityTags(NextSpec.AbilityTags))
            {
                return false;
            }
            if (NextSpec.AbilityCost is not null && !NextSpec.AbilityCost.CanConsumeCost())
            {
                return false;
            }
            return true;
        }
    }
}