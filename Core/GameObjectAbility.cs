using UnityEngine;

namespace StudioScor.AbilitySystem
{
    [CreateAssetMenu(menuName ="StudioScor/AbilitySystem/new GameObject Ability" , fileName = "GA_")]
    public class GameObjectAbility : Ability
    {
        [Header(" [ GameObject Ability ] ")]
        [SerializeField] private GameObjectAbilitySpec _abilitySpec;

        public override IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0)
        {
            var spec = Instantiate(_abilitySpec, abilitySystem.transform);

            spec.Setup(this, abilitySystem, level);

            return spec;
        }
    }
}
