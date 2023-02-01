using StudioScor.Utilities;
using UnityEngine;

namespace StudioScor.AbilitySystem
{
    public abstract class AbilityTaskSpec<T> : BaseClass, IAbilityTaskSpec where T : AbilityTask
    {
        private readonly T _AbilityTask;
        private readonly IAbilitySpec _AbilitySpec;

        private bool _IsPlaying;

        public new bool UseDebug => _AbilityTask.UseDebug;
        public new Object Context => _AbilityTask;

        protected T AbilityTask => _AbilityTask;
        protected IAbilitySpec AbilitySpec => _AbilitySpec;
        protected bool IsPlaying => _IsPlaying;


        protected AbilityTaskSpec(T actionBlock, IAbilitySpec abilitySpec)
        {
            _AbilityTask = actionBlock;
            _AbilitySpec = abilitySpec;
        }

        public void Remove()
        {
            OnRemove();
        }

        public virtual bool CanActivateTask()
        {
            return _AbilityTask.IsAlwaysPass || CanEnterTask();
        }
        

        public void ForceEnterTask()
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;

            EnterTask();
        }
        public void EndTask()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;

            ExitTask();
        }
        
        public bool TryEnterTask()
        {
            if (CanEnterTask())
            {
                ForceEnterTask();

                return true;
            }

            return false;
        }

        public void OnUpdateTask(float normalizedTime)
        {
            if (!IsPlaying)
                return;

            UpdateTask(normalizedTime);
        }

        protected virtual void OnRemove() { }
        public virtual bool CanEnterTask()
        {
            return !IsPlaying;
        }

        protected abstract void EnterTask();
        protected virtual void ExitTask() { }
        protected virtual void UpdateTask(float normalizedTime) { }
    }
}
