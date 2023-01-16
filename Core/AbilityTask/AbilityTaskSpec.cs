using System.Diagnostics;

namespace StudioScor.AbilitySystem
{
    public abstract class AbilityTaskSpec
    {
        private readonly AbilityTask _AbilityTask;
        private readonly IAbilitySpec _AbilitySpec;

        private bool _IsPlaying;

#region EDITOR ONLY
        [Conditional("UNITY_EDITOR")]
        protected void Log(object content, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                UnityEngine.Debug.LogError(_AbilitySpec.Ability.name + " [ " + GetType().Name + " ] : " + content, _AbilitySpec.Ability);

                return;
            }

            if (_AbilityTask.UseDebug)
                UnityEngine.Debug.Log(_AbilitySpec.Ability.name + " [ " + GetType().Name + " ] : " + content, _AbilitySpec.Ability);
#endif
        }
#endregion

        protected AbilityTask AbilityTask => _AbilityTask;
        protected IAbilitySpec AbilitySpec => _AbilitySpec;
        protected bool IsPlaying => _IsPlaying;


        protected AbilityTaskSpec(AbilityTask actionBlock, IAbilitySpec abilitySpec)
        {
            _AbilityTask = actionBlock;
            _AbilitySpec = abilitySpec;
        }

        public void Remove()
        {
            OnRemove();
        }

        public virtual bool CanAction()
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

        public void OnUpdateAction(float normalizedTime)
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
