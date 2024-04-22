using StudioScor.Utilities;
using System;
using UnityEngine;

namespace StudioScor.AbilitySystem.Variable
{
    [Serializable]
    public class AbilityLevelVariable : IntegerVariable
    {
        [Header(" [ Ability Level Variable ] ")]
        [SerializeField] private Ability _ability;

        private IAbilitySystem _abilitySystem;

        private AbilityLevelVariable _original;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _abilitySystem = Owner.GetAbilitySystem();
        }
        public override IIntegerVariable Clone()
        {
            var clone = new AbilityLevelVariable();

            clone._original = this;

            return clone;
        }

        public override int GetValue()
        {
            if(_abilitySystem.TryGetAbilitySpec(_original is null ? _ability : _original._ability, out IAbilitySpec spec))
            {
                return spec.Level;
            }

            return -1;
        }
    }
}
