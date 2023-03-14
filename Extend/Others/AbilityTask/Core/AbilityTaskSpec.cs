using StudioScor.Utilities;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;



namespace StudioScor.AbilitySystem
{
    public abstract class TaskSpec : BaseClass, ITaskSpec
    {
        protected readonly Task _Task;
        protected readonly GameObject _Owner;

        protected bool _IsPlaying;
        protected float _Strength = 1f;
        protected bool _IsSubTask = false;
        private readonly List<ITaskSpec> _SubTasks;


#if UNITY_EDITOR
        public override bool UseDebug => _Task.UseDebug;
        public override Object Context => _Task;
#endif

        public Task Task => _Task;
        public GameObject Owner => _Owner;
        public bool IsPlaying => _IsPlaying;
        public abstract float Progress { get; }
        public bool IsSubTask => _IsSubTask;
        public float Strength => _Strength;

       

        public event TaskEventHandler OnStartedTask;
        public event TaskEventHandler OnFinishedTask;

        protected TaskSpec(Task task, GameObject owner)
        {
            _Task = task;
            _Owner = owner;
            _SubTasks = new();

            for(int i = 0; i < _Task.SubTasks.Count; i++)
            {
                var subTask = _Task.SubTasks.ElementAt(i).CreateSpec(owner);

                subTask.SetUseSubTask(true);

                _SubTasks.Add(subTask);
            }
        }

        public void Remove()
        {
            OnRemove();
        }

        public void AddTask(ITaskSpec taskSpec)
        {
            if (_SubTasks.Contains(taskSpec))
                return;

            _SubTasks.Add(taskSpec);

            taskSpec.SetUseSubTask(true);

            Log($"Add New Task -  {taskSpec}");
        }
        public void RemoveTask(ITaskSpec taskSpec)
        {
            if(_SubTasks.Remove(taskSpec))
            {
                taskSpec.SetUseSubTask(false);
            }
        }

        public virtual bool CanActivateTask()
        {
            if (!_Task.IsAlwaysPass && !CanEnterTask())
                return false;

            if(_SubTasks.Count > 0)
            {
                foreach (var subTask in _SubTasks)
                {
                    if (!subTask.CanActivateTask())
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

        public void OnUpdateTask(float time)
        {
            if (!IsPlaying)
                return;

            if(IsSubTask)
            {
                UpdateSubTask(time);
            }
            else
            {
                UpdateMainTask(time);
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

        public void SetStrength(float strength)
        {
            _Strength = strength;

            foreach (var task in _SubTasks)
            {
                task.SetStrength(_Strength);
            }
        }

        public void SetUseSubTask(bool isSubTask)
        {
            _IsSubTask = isSubTask;
        }

        protected abstract void EnterTask();
        protected virtual void ExitTask() { }
        protected virtual void UpdateSubTask(float progress) { }
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
