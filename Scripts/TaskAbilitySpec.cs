using System.Collections.Generic;
using System.Linq;

namespace KimScor.GameplayTagSystem.Ability
{
    public class TaskAbilitySpec : AbilitySpec
    {
        private TaskAbility _TaskAbility;

        private FAbilityTaskSpecContainer[] _AbilityTaskSpecs;
        public IReadOnlyCollection<FAbilityTaskSpecContainer> AbilityTaskSpecs => _AbilityTaskSpecs;

        public FAbilityTaskSpecContainer PrevSpec => AbilityTaskSpecs.ElementAt(_CurrentNumber - 1);
        public FAbilityTaskSpecContainer CurrentSpec => AbilityTaskSpecs.ElementAt(_CurrentNumber);
        public FAbilityTaskSpecContainer NextSpec => AbilityTaskSpecs.ElementAt(_CurrentNumber + 1);

        private int _CurrentNumber = 0;
        public int CurrentNumber => _CurrentNumber;
        public bool CanNextSpec => CurrentNumber < AbilityTaskSpecs.Count - 1;

        public TaskAbilitySpec(Ability ability, AbilitySystem owner, int level) : base(ability, owner, level)
        {
            _TaskAbility = ability as TaskAbility;
        }

        public override void OnGrantAbility()
        {
            _AbilityTaskSpecs = new FAbilityTaskSpecContainer[_TaskAbility.AbilityTasks.Count];

            for (int i = 0; i < _AbilityTaskSpecs.Length; i++)
            {
                var specs = new List<AbilityTaskSpec>();

                foreach (var task in _TaskAbility.AbilityTasks.ElementAt(i).AbilityTasks)
                {
                    specs.Add(task.CreateSpec(Owner.GameplayTagSystem, this));
                }

                _AbilityTaskSpecs[i].ActivateTag = _TaskAbility.AbilityTasks.ElementAt(i).ActivateTag;
                _AbilityTaskSpecs[i].DeActiveTag = _TaskAbility.AbilityTasks.ElementAt(i).DeActiveTag;
                _AbilityTaskSpecs[i].ReTriggerTag = _TaskAbility.AbilityTasks.ElementAt(i).ReTriggerTag;
                _AbilityTaskSpecs[i].AbilityTags = _TaskAbility.AbilityTasks.ElementAt(i).AbilityTags;
                _AbilityTaskSpecs[i].AbilityTaskSpecs = specs.ToArray();
            }

            GameplayTagSystem.OnTriggerTag += GameplayTagSystem_OnTriggerTag;
        }

        private void GameplayTagSystem_OnTriggerTag(GameplayTagSystem gameplayTagSystem, GameplayTag changedTag)
        {
            if (changedTag == CurrentSpec.ReTriggerTag)
            {
                TryReTriggerAbility();
            }
            else if (changedTag == CurrentSpec.DeActiveTag)
            {
                EndAbility();
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

            PlayAbility();
        }
        private void PlayAbility()
        {
            GameplayTagSystem.AddOwnedTags(CurrentSpec.AbilityTags.ActivationOwnedTags);
            GameplayTagSystem.AddBlockTags(CurrentSpec.AbilityTags.ActivationBlockedTags);

            Owner.OnCancelAbility(CurrentSpec.AbilityTags.CancelAbilitiesWithTag);

            GameplayTagSystem.TriggerTag(CurrentSpec.ActivateTag);
        }

        private bool CheckAbilityTags(FAbilityTags abilityTags)
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

            return true;
        }
        public override bool CanReTriggerAbility()
        {
            if (!CanNextSpec)
            {
                return false;
            }
            if (!CheckAbilityTags(NextSpec.AbilityTags))
            {
                return false;
            }

            return true;
        }
    }
}