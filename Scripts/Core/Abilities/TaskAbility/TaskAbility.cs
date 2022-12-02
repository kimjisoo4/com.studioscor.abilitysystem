using System.Collections.Generic;
using UnityEngine;

namespace KimScor.GameplayTagSystem.Ability
{
    [CreateAssetMenu(menuName = "GAS/Ability/New Task Ability Test", fileName = "GA_")]
    public class TaskAbility : Ability
    {
        [SerializeField] private FAbilityTaskContainer[] _AbilityTasks;

        public IReadOnlyCollection<FAbilityTaskContainer> AbilityTasks => _AbilityTasks;

        public override IAbilitySpec CreateSpec(AbilitySystem owner, int level)
        {
            var spec = new TaskAbilitySpec(this, owner, level);

            return spec;
        }
    }
}
