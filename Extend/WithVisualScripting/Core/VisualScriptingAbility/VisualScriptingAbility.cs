﻿#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;


namespace StudioScor.AbilitySystem.VisualScripting
{

    [CreateAssetMenu(menuName ="StudioScor/AbilitySystem/new VisualScriptingAbility", fileName = "GA_")]
    public class VisualScriptingAbility : Ability
    {
        [Header(" [ Visual Scripting Ability ] ")]
        [SerializeField] private VisualScriptingAbilitySpec _VisualScriptingAbilitySpec;

        public override IAbilitySpec CreateSpec(IAbilitySystem abilitySystem, int level = 0)
        {
            var abilitySpec = Instantiate(_VisualScriptingAbilitySpec, abilitySystem.transform);

            abilitySpec.Setup(this, abilitySystem, level);

            return abilitySpec;
        }
    }
}

#endif