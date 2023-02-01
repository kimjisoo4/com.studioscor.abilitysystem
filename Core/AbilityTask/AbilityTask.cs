using UnityEngine;
using StudioScor.Utilities;

namespace StudioScor.AbilitySystem
{

    public abstract class AbilityTask : BaseScriptableObject
    {
        [Header(" [ Ability Task ] ")]
        [SerializeField] private bool _IsAlwaysPass = false;
        public bool IsAlwaysPass => _IsAlwaysPass;


        public abstract IAbilityTaskSpec CreateSpec(IAbilitySpec abilitySpec);
    }
}
