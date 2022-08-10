using System.Collections.Generic;
using UnityEngine;

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class AbilityTask : ScriptableObject
    {
        [Header("[ Task Active Tags ]")]
        [SerializeField] private AbilityTaskTag _ActiveTag;

        [Header("[ Task Start, Finished Trigger Tags ]")]
        [SerializeField] private AbilityTaskTag[] _StartedTags;
        [SerializeField] private AbilityTaskTag[] _FinishedTags;
        public AbilityTaskTag ActiveTag => _ActiveTag;
        public IReadOnlyCollection<AbilityTaskTag> StartedTags => _StartedTags;
        public IReadOnlyCollection<AbilityTaskTag> FinishedTags => _FinishedTags;
        public abstract AbilityTaskSpec CreateSpec(TaskAbilitySpec taskAbilitySpec);
    }
}