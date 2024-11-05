#if SCOR_ENABLE_BEHAVIOR
using UnityEngine;
using StudioScor.Utilities;
using Unity.Behavior;
using System;
using Unity.Properties;

namespace StudioScor.AbilitySystem.Behavior
{
    [Serializable, GeneratePropertyBag]
    [Condition(name: "Has Ability",
        story: "[Self] Has [Ability] Ability ( UseDebug [UseDebugKey] )",
        category: "Conditions/StudioScor/AbilitySystem",
        id: "playersystem_hasability")]
    public partial class HasAbilityCondition : AbilitySystemCondition
    {
        [SerializeReference] public BlackboardVariable<Ability> Ability = new();
        public override bool IsTrue()
        {
            if (!base.IsTrue())
                return false;

            if (!Ability.Value)
            {
                Log($"{nameof(Ability).ToBold()} Is {"Null".ToBold().ToColor(SUtility.STRING_COLOR_FAIL)}");
                return false;
            }

            var result = _abilitySystem.HasAbility(Ability.Value);

            Log($"{nameof(Self).ToBold()} Is {(result ? $"Has {Ability.Value}".ToColor(SUtility.STRING_COLOR_SUCCESS) : $"Not Has {Ability.Value}".ToColor(SUtility.STRING_COLOR_FAIL)).ToBold()}");

            return result;
        }
    }
}

#endif