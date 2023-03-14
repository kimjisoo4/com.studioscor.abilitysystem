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


        public virtual bool CanGrantAbility(IAbilitySystem abilitySystem)
        {
            return true;
        }

        public abstract IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0);
    }
}
