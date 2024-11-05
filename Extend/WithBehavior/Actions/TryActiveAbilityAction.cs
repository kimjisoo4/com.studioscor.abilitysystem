#if SCOR_ENABLE_BEHAVIOR
using UnityEngine;
using StudioScor.Utilities;
using Unity.Behavior;
using System;

namespace StudioScor.AbilitySystem.Behavior
{

    [Serializable, Unity.Properties.GeneratePropertyBag]
    [NodeDescription(name: "Try Active Ability", story: "[Self] Try Active [Ability] Ability", category: "Action/StudioScor/AbilitySystem", id: "playersystem_tryactiveability")]
    public class TryActiveAbilityAction : AbilitySystemAction
    {
        [SerializeReference] public BlackboardVariable<Ability> Ability;

        private Status _result;

        protected override Status OnStart()
        {
            if (base.OnStart() == Status.Failure)
                return Status.Failure;

            if (!Ability.Value)
            {
                Log($"{nameof(Ability).ToBold()} is {"Null".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");
                return Status.Failure;
            }

            if (_abilitySystem.TryGetAbilitySpec(Ability.Value, out IAbilitySpec spec))
            {
                SetupSpec(spec);

                if (spec.CanActiveAbility())
                {
                    spec.OnCanceledAbility += Spec_OnCanceledAbility;
                    spec.OnFinishedAbility += Spec_OnFinishedAbility;

                    _result = Status.Running;

                    spec.ForceActiveAbility();
                }
                else
                {
                    Log($"{"Can Not".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)} activate {nameof(Ability.Value).ToBold()}.");

                    _result = Status.Failure;
                }
            }
            else
            {
                Log($"{nameof(Self).ToBold()} is {$"Not has {nameof(Ability.Value)}".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}.");

                _result = Status.Failure;
            }

            return _result;
        }

        private void Spec_OnFinishedAbility(IAbilitySpec abilitySpec)
        {
            Log($"On Finished Ability".ToBold().ToColor(SUtility.STRING_COLOR_SUCCESS));
            abilitySpec.OnFinishedAbility -= Spec_OnFinishedAbility;
            abilitySpec.OnCanceledAbility -= Spec_OnCanceledAbility;

            _result = Status.Success;
        }

        private void Spec_OnCanceledAbility(IAbilitySpec abilitySpec)
        {
            Log($"On Canceled Ability".ToBold().ToColor(SUtility.STRING_COLOR_FAIL));
            abilitySpec.OnFinishedAbility -= Spec_OnFinishedAbility;
            abilitySpec.OnCanceledAbility -= Spec_OnCanceledAbility;

            _result = Status.Failure;
        }

        protected override Status OnUpdate()
        {
            Debug.Log(_result);
            return _result;
        }

        protected virtual void SetupSpec(IAbilitySpec _abilitySpec)
        {

        }
    }
}

#endif