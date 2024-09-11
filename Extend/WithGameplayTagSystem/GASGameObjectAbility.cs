#if SCOR_ENABLE_GAMEPLAYTAGSYSTEM
using UnityEngine;

namespace StudioScor.AbilitySystem
{
    [CreateAssetMenu(menuName = "StudioScor/AbilitySystem/new GAS GameObject Ability", fileName = "GA_")]
    public class GASGameObjectAbility : GASAbility
    {
        [Header(" [ GAS GameObject Ability ] ")]
        [SerializeField] private GASGameObjectAbilitySpec _abilitySpec;

        public override IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0)
        {
            var spec = Instantiate(_abilitySpec, abilitySystem.transform);

            spec.Setup(this, abilitySystem, level);

            return spec;
        }
    }
}
#endif