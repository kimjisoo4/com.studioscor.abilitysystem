using StudioScor.Utilities;
using UnityEngine;
using System.Linq;



namespace StudioScor.AbilitySystem
{
    public abstract class AbilityTaskSpec<T> : BaseClass, IAbilityTaskSpec where T : AbilityTask
    {
        private readonly T _AbilityTask;
        private readonly IAbilitySpec _AbilitySpec;

        private bool _IsPlaying;

        public event AbilityTaskEventHandler OnStartedTask;
        public event AbilityTaskEventHandler OnFinishedTask;

#if UNITY_EDITOR
        public override bool UseDebug => _AbilityTask.UseDebug;
        public override Object Context => _AbilityTask;
#endif

        protected T AbilityTask => _AbilityTask;
        protected IAbilitySpec AbilitySpec => _AbilitySpec;
        public bool IsPlaying => _IsPlaying;
        public abstract float Progress { get; }
        public bool IsSub { get; set; } = false;

        private readonly IAbilityTaskSpec[] _SubTasks;

        protected AbilityTaskSpec(T actionBlock, IAbilitySpec abilitySpec)
        {
            _AbilityTask = actionBlock;
            _AbilitySpec = abilitySpec;

            if(_AbilityTask.SubTasks is null)
            {
                _SubTasks = System.Array.Empty<IAbilityTaskSpec>();

                return;
            }

            _SubTasks = new IAbilityTaskSpec[_AbilityTask.SubTasks.Count];

            for(int i = 0; i < _AbilityTask.SubTasks.Count; i++)
            {
                _SubTasks[i] = _AbilityTask.SubTasks.ElementAt(i).CreateSpec(abilitySpec);
                _SubTasks[i].IsSub = true;
            }
        }

        public void Remove()
        {
            OnRemove();
        }

        public virtual bool CanActivateTask()
        {
            if (!_AbilityTask.IsAlwaysPass && !CanEnterTask())
                return false;

            if(_SubTasks.Length > 0)
            {
                foreach (var subTask in _SubTasks)
                {
                    if (!subTask.CanEnterTask())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        

        public void ForceEnterTask()
        {
            if (_IsPlaying)
                return;

            _IsPlaying = true;

            EnterTask();

            foreach (var subTask in _SubTasks)
            {
                subTask.TryEnterTask();
            }

            Callback_OnStartedTask();
        }
        public void EndTask()
        {
            if (!_IsPlaying)
                return;

            _IsPlaying = false;

            ExitTask();

            foreach (var subTask in _SubTasks)
            {
                subTask.EndTask();
            }

            Callback_OnFisnihedTask();
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

        public void OnUpdateTask(float deltaTime)
        {
            if (!IsPlaying)
                return;

            if(IsSub)
            {
                UpdateSubTask(deltaTime);
            }
            else
            {
                UpdateMainTask(deltaTime);
            }

            foreach (var subTask in _SubTasks)
            {
                subTask.OnUpdateTask(Progress);
            }
        }

        protected virtual void OnRemove() { }
        public virtual bool CanEnterTask()
        {
            return !IsPlaying;
        }

        protected abstract void EnterTask();
        protected virtual void ExitTask() { }
        protected virtual void UpdateSubTask(float normalizedTime) { }
        protected virtual void UpdateMainTask(float deltaTime) { }

        #region Callback
        protected void Callback_OnStartedTask()
        {
            Log("On Started Task");

            OnStartedTask?.Invoke(this);
        }
        protected void Callback_OnFisnihedTask()
        {
            Log("On Finished Task");

            OnFinishedTask?.Invoke(this);
        }
        #endregion
    }
}
