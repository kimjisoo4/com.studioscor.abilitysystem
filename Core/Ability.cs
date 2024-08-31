using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.AbilitySystem
{
    public abstract class Ability : BaseScriptableObject
    {
        [Header(" [ Ability ] ")]
        [SerializeField] protected string _id;
        public string ID => _id;

        [ContextMenu(nameof(NameToID))]
        private void NameToID()
        {
            _id = name;
        }


        public virtual bool CanGrantAbility(IAbilitySystem abilitySystem)
        {
            return true;
        }

        public abstract IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0);
    }
}
