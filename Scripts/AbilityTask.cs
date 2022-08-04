using System.Collections.Generic;
using UnityEngine;

using KimScor.GameplayTagSystem;
using KimScor.GameplayTagSystem.Ability;

public abstract class AbilityTask : ScriptableObject
{
    [Header("[ Task Active Tags ]")]
    [SerializeField] private GameplayTag _ActiveTag;

    [Header("[ Task Start, Finished Trigger Tags ]")]
    [SerializeField] private GameplayTag[] _StartedTags;
    [SerializeField] private GameplayTag[] _FinishedTags;
    public GameplayTag ActiveTag => _ActiveTag;
    public IReadOnlyCollection<GameplayTag> StartedTags => _StartedTags;
    public IReadOnlyCollection<GameplayTag> FinishedTags => _FinishedTags;
    public abstract AbilityTaskSpec CreateSpec(GameplayTagSystem gameplayTagSystem, TaskAbilitySpec taskAbilitySpec);
}
