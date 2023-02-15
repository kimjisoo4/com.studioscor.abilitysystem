namespace StudioScor.AbilitySystem
{
    public delegate void AbilityTaskEventHandler(IAbilityTaskSpec abilityTaskSpec);
    public interface IAbilityTaskSpec
    {
        public void Remove();
        public bool CanActivateTask();
        public void ForceEnterTask();
        public bool CanEnterTask();
        public bool TryEnterTask();
        public void EndTask();
        public void OnUpdateTask(float deltaTime);
        public float Progress { get; }
        public bool IsPlaying { get; }
        public bool IsSub { get; set; }

        public event AbilityTaskEventHandler OnStartedTask;
        public event AbilityTaskEventHandler OnFinishedTask;
    }
}
