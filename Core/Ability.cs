using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.AbilitySystem
{
    public abstract class Ability : BaseScriptableObject
    {
        [Header(" [ Ability ] ")]
        [SerializeField] protected string abilityName;
        public string AbilityName => abilityName;


        public virtual bool CanGrantAbility(IAbilitySystem abilitySystem)
        {
            return true;
        }

        public abstract IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0);
    }
}
