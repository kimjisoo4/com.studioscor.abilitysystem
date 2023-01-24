using UnityEngine;

namespace StudioScor.AbilitySystem
{

    public abstract class AbilityTask 
    {
        [Header(" [ Ability Task ] ")]
        [SerializeField] private bool _IsAlwaysPass = false;
        public bool IsAlwaysPass => _IsAlwaysPass;

        #region EDITOR ONLY
#if UNITY_EDITOR
        [Header(" [ Use Debug ] ")]

        [SerializeField] private bool _UseDebug = false;
        public bool UseDebug => _UseDebug;
#endif
        #endregion

        public abstract IAbilityTaskSpec CreateSpec(IAbilitySpec abilitySpec);
    }
}
