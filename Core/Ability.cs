using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.AbilitySystem
{
    public interface IAbility
    {
        public string ID { get; }
        public bool CanGrantAbility(IAbilitySystem abilitySystem);
        public abstract IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0);
    }

    public abstract class Ability : BaseScriptableObject, IAbility
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
