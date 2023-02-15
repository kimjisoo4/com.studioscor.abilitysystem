using UnityEngine;
using System.Collections.Generic;
using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{

    public abstract class AbilityTask : BaseScriptableObject
    {
        [Header(" [ Ability Task ] ")]
        [SerializeField] private bool _IsAlwaysPass = false;
        [SerializeField] private AbilityTask[] _SubTasks = null;

        public bool IsAlwaysPass => _IsAlwaysPass;
        public IReadOnlyCollection<AbilityTask> SubTasks => _SubTasks;

        public abstract IAbilityTaskSpec CreateSpec(IAbilitySpec abilitySpec);
    }
}
