using UnityEngine;
using System.Collections.Generic;
using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{

    public abstract class Task : BaseScriptableObject
    {
        [Header(" [ Ability Task ] ")]
        [SerializeField] private bool _IsAlwaysPass = false;
        [SerializeField] private Task[] _SubTasks = null;
        public bool IsAlwaysPass => _IsAlwaysPass;
        public IReadOnlyCollection<Task> SubTasks => _SubTasks;

        public abstract ITaskSpec CreateSpec(GameObject owner);
    }
}
