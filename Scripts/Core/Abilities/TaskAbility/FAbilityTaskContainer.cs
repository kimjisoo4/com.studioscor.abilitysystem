namespace KimScor.GameplayTagSystem.Ability
{
    [System.Serializable]
    public struct FAbilityTaskContainer
    {
#if UNITY_EDITOR
        public string TaskName;
#endif
        public AbilityTaskTag ActivateTag;
        public AbilityTaskTag EndAbilityTag;
        public AbilityTaskTag ReTriggerTag;
        public AbilityTaskTag CanNextTaskTag;

        public FAbilityTags AbilityTags;
        public AbilityCost AbilityCost;
        public AbilityTask[] AbilityTasks;

        
    }

}