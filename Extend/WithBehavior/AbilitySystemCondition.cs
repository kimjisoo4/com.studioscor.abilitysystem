#if SCOR_ENABLE_BEHAVIOR
using UnityEngine;
using StudioScor.Utilities;
using StudioScor.Utilities.UnityBehavior;
using Unity.Behavior;

namespace StudioScor.AbilitySystem.Behavior
{
    public abstract class AbilitySystemCondition : BaseCondition
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;

        protected IAbilitySystem _abilitySystem;

        public override void OnStart()
        {
            base.OnStart();

            var self = Self.Value;

            if (!self)
            {
                _abilitySystem = null;
                LogError($"{nameof(Self).ToBold()} is Null.");
                return;
            }

            if (!self.TryGetAbilitySystem(out _abilitySystem))
            {
                LogError($"{nameof(Self).ToBold()} is Not Has {nameof(IAbilitySystem).ToBold()}.");
                return;
            }
        }

        public override bool IsTrue()
        {
            var result = _abilitySystem is not null;

            Log($"{nameof(Self).ToBold()} {(result ? $"Has {nameof(IAbilitySystem)}".ToColor(SUtility.STRING_COLOR_SUCCESS) : $"Not Has {nameof(IAbilitySystem).ToColor(SUtility.STRING_COLOR_FAIL)}").ToBold()}");

            return result;
        }
    }
}

#endif