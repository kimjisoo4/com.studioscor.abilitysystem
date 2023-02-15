using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.AbilitySystem
{
    public abstract class Ability : BaseScriptableObject
    {
        [Header(" [ Ability ] ")]
        [SerializeField] protected string _Name;
        [SerializeField] protected Sprite _Icon;
        
        public string Name => _Name;
        public Sprite Icon => _Icon;


        public virtual bool CanGrantAbility(AbilitySystemComponent abilitySystem)
        {
            return true;
        }

        public abstract IAbilitySpec CreateSpec(AbilitySystemComponent abilitySystem, int level = 0);
    }
}
