
namespace KimScor.GameplayTagSystem.Ability
{
    public struct FAbilityTaskSpecContainer
    {
        public AbilityTaskTag ActivateTag;
        public AbilityTaskTag EndAbilityTag;
        public AbilityTaskTag ReTriggerTag;
        public AbilityTaskTag CanNextTaskTag;

        public FAbilityTags AbilityTags;
        public AbilityCostSpec AbilityCost;
        public AbilityTaskSpec[] AbilityTaskSpecs;
        
    }
}
