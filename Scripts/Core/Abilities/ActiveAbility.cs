using UnityEngine;

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class ActiveAbility : Ability
    {
        [Header("[Use Gameplay Tag]")]
        [SerializeField] protected FAbilityTags _AbilityTags;
        public FAbilityTags AbilityTags => _AbilityTags;
    }
}
