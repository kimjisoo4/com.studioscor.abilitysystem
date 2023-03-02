namespace StudioScor.AbilitySystem
{
    public delegate void TaskEventHandler(ITaskSpec taskSpec);

    public interface ITaskSpec
    {
        public void Remove();
        public bool CanActivateTask();
        public void ForceEnterTask();
        public bool CanEnterTask();
        public bool TryEnterTask();
        public void EndTask();
        public void OnUpdateTask(float deltaTime);
        public void SetStrength(float strength);
        public void SetUseSubTask(bool useSubTask);
        public float Progress { get; }
        public bool IsPlaying { get; }
        public bool IsSubTask { get; }
        public float Strength { get; }

        public event TaskEventHandler OnStartedTask;
        public event TaskEventHandler OnFinishedTask;
    }
}
