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
            GameplayTagSystem.RemoveOwnedTags(CurrentSpec.AbilityTags.AddOwnedTags);
            GameplayTagSystem.RemoveBlockTags(CurrentSpec.AbilityTags.ObstacledTags);

            _CurrentNumber = 0;
            _CanNextTask = false;
        }

        protected override void EnterAbility()
        {
            PlayAbility();
        }

        protected override void FixedUpdateAbility(float deltaTime)
        {
            base.FixedUpdateAbility(deltaTime);

            foreach (AbilityTaskSpec abilitySpec in CurrentSpec.AbilityTaskSpecs)
            {
                abilitySpec.UpdateTask(deltaTime);
            }
        }

        protected override void ExitAbility()
        {
            GameplayTagSystem.RemoveOwnedTags(CurrentSpec.AbilityTags.AddOwnedTags);
            GameplayTagSystem.RemoveBlockTags(CurrentSpec.AbilityTags.ObstacledTags);

            _CurrentNumber = 0;
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

            GameplayTagSystem.RemoveOwnedTags(PrevSpec.AbilityTags.AddOwnedTags);
            GameplayTagSystem.RemoveBlockTags(PrevSpec.AbilityTags.ObstacledTags);

            _CanNextTask = false;

            PlayAbility();
        }
        private void PlayAbility()
        {
            if (CurrentSpec.AbilityCost is not null)
            {
                CurrentSpec.AbilityCost.ConsumeCost();
            }

            GameplayTagSystem.AddOwnedTags(CurrentSpec.AbilityTags.AddOwnedTags);
            GameplayTagSystem.AddBlockTags(CurrentSpec.AbilityTags.ObstacledTags);

            Owner.OnCancelAbility(CurrentSpec.AbilityTags.CancelAbilityTags);

            OnTriggerTag(CurrentSpec.ActivateTag);
        }

        protected bool CheckAbilityTags(FAbilityTags abilityTags)
        {
            // 해당 태그가 모두 존재하고 있는가
            if (abilityTags.RequiredTags is not null
                && !GameplayTagSystem.ContainAllTagsInOwned(abilityTags.RequiredTags))
            {
                Log("필수 태그를 소유하고 있지 않음");

                return false;
            }

            // 해당 태그를 가지고 있는가
            if (abilityTags.ObstacledTags is not null
                && !GameplayTagSystem.ContainOnceTagsInOwned(abilityTags.ObstacledTags))
            {
                Log("방해 태그를 소유하고 있음");

                return false;
            }

            return true;
        }

        public override bool CanActiveAbility()
        {
            // 어빌리티 태그가 블록 되어 있는가
            if (Ability.AbilityTag is not null
                && GameplayTagSystem.ContainBlockTag(Ability.AbilityTag))
            {
                Log("어빌리티가 블록되어 있음.");

                return false;
            }

            // 속성 태그 중에 블록되어 있는게 있는가
            if (Ability.AttributeTags is not null
                && !GameplayTagSystem.ContainOnceTagsInBlock(Ability.AttributeTags.ToArray()))
            {
                Log("어빌리티의 속성이 블록되어 있음.");

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