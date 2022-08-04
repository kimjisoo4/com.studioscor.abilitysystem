namespace KimScor.GameplayTagSystem.Ability
{
    [System.Serializable]
    public struct FAbilityTaskContainer
    {
#if UNITY_EDITOR
        public string TaskName;
#endif
        public GameplayTag ActivateTag;
        public GameplayTag DeActiveTag;
        public GameplayTag ReTriggerTag;
        public FAbilityTags AbilityTags;
        public AbilityTask[] AbilityTasks;
    }

}