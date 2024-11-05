#if SCOR_ENABLE_BEHAVIOR
using UnityEngine;
using StudioScor.Utilities;
using StudioScor.Utilities.UnityBehavior;
using Unity.Behavior;

namespace StudioScor.AbilitySystem.Behavior
{
    public class AbilitySystemAction : BaseAction
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;

        protected IAbilitySystem _abilitySystem;

        protected override Status OnStart()
        {
            var self = Self.Value;

            if (!self)
            {
                LogError($"{nameof(Self).ToBold()} is {"Null".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");
                return Status.Failure;
            }

            if (!self.TryGetAbilitySystem(out _abilitySystem))
            {
                LogError($"{nameof(Self).ToBold()} is Not Has {nameof(_abilitySystem).ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");
                return Status.Failure;
            }

            return Status.Running;
        }
    }
}

#endif