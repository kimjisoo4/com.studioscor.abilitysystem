#if SCOR_ENABLE_BEHAVIOR
using UnityEngine;
using StudioScor.Utilities;
using Unity.Behavior;
using System;

namespace StudioScor.AbilitySystem.Behavior
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [NodeDescription(name: "Remove Ability", story: "[Self] Remove [Ability] Ability ", category: "Action/StudioScor/AbilitySystem", id: "playersystem_removeability")]
    public class RemoveAbilityAction : AbilitySystemAction
    {
        [SerializeReference] public BlackboardVariable<Ability> Ability;

        protected override Status OnStart()
        {
            if (base.OnStart() == Status.Failure)
                return Status.Failure;

            if (!Ability.Value)
            {
                Log($"{nameof(Ability).ToBold()} is {"Null".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");
                return Status.Failure;
            }

            if (_abilitySystem.RemoveAbility(Ability.Value))
            {
                return Status.Success;
            }
            else
            {
                Log($"{nameof(Self).ToBold()} is {$"Failed to Remove {nameof(Ability.Value)}".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");
                return Status.Failure;
            }
        }
    }
}

#endif