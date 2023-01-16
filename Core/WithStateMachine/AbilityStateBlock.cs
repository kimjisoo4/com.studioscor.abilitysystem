using UnityEngine;
using StudioScor.GameplayTagSystem;


namespace StudioScor.AbilitySystem
{
    public abstract class AbilityStateBlock : ScriptableObject
    {
        #region EDITOR ONLY
#if UNITY_EDITOR
        public string NewName = "State";

        private void OnValidate()
        {
            if(this.name != NewName)
                this.name = NewName;
        }
#endif
        #endregion

        [field: Header(" [ Ability State ] ")]
        [field: SerializeField] public FConditionTags ConditionTags { get; private set; }
        [field: SerializeField] public FGameplayTags GrantTags { get; private set; }

        [field: Header(" [ Use Debug ] ")]
        [field: SerializeField] public bool UseDebug { get; private set; } = false;

        public abstract AbilityStateBlockBaseSpec CreateSpec(IAbilityState abilityState);
    }
}
