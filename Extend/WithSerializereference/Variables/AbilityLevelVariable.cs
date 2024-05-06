using StudioScor.Utilities;
using System;
using UnityEngine;

namespace StudioScor.AbilitySystem.Variable
{
    [Serializable]
    public class AbilityLevelVariable : IntegerVariable
    {
        [Header(" [ Ability Level Variable ] ")]
#if SCOR_ENABLE_SERIALIZEREFERENCE
        [SerializeReference, SerializeReferenceDropdown]
#endif
        private IGameObjectVariable _target = new SelfGameObjectVariable();
        [SerializeField] private Ability _ability;

        private IAbilitySystem _abilitySystem;

        private AbilityLevelVariable _original;

        public override void Setup(GameObject owner)
        {
            base.Setup(owner);

            _target.Setup(owner);

            var target = _target.GetValue();

            _abilitySystem = target.GetAbilitySystem();
        }

        public override IIntegerVariable Clone()
        {
            var clone = new AbilityLevelVariable();

            clone._original = this;
            clone._target = _target.Clone();

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
