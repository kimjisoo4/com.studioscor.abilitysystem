#if SCOR_ENABLE_BEHAVIOR
using UnityEngine;
using StudioScor.Utilities;
using Unity.Behavior;
using System;

namespace StudioScor.AbilitySystem.Behavior
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [NodeDescription(name: "Try Grant Ability", story: "[Self] Try Grant [Ability] Ability", category: "Action/StudioScor/AbilitySystem", id: "playersystem_trygrantability")]
    public class TryGrantAbilityAction : AbilitySystemAction
    {
        [SerializeReference] public BlackboardVariable<Ability> Ability;
        [SerializeReference] public BlackboardVariable<int> Level = new(0);

        protected override Status OnStart()
        {
            if (base.OnStart() == Status.Failure)
                return Status.Failure;

            if (!Ability.Value)
            {
                Log($"{nameof(Ability).ToBold()} is {"Null".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");
                return Status.Failure;
            }

            if (_abilitySystem.TryGrantAbility(Ability.Value, Level.Value))
            {
                return Status.Success;
            }
            else
            {
                Log($"{nameof(Self).ToBold()} is {$"Failed to Grant {nameof(Ability.Value)}".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");
                return Status.Failure;
            }
        }
    }
}

#endif