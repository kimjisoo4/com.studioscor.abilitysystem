namespace StudioScor.AbilitySystem
{
    public interface IAbilityTaskSpec
    {
        public void Remove();
        public bool CanActivateTask();
        public void ForceEnterTask();
        public void EndTask();
        public bool TryEnterTask();
        public void OnUpdateTask(float normalizedTime);
        public bool CanEnterTask();
    }
}
