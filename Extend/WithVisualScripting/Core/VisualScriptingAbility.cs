#if SCOR_ENABLE_VISUALSCRIPTING
using UnityEngine;


namespace StudioScor.AbilitySystem.VisualScripting
{

    [CreateAssetMenu(menuName ="StudioScor/AbilitySystem/new VisualScriptingAbility", fileName = "GA_")]
    public class VisualScriptingAbility : Ability
    {
        [Header(" [ Visual Scripting Ability ] ")]
        [SerializeField] private AbilitySpecWithVisualScripting _VisualScriptingAbilitySpec;

        public override IAbilitySpec CreateSpec(AbilitySystemComponent abilitySystemComponent, int level = 0)
        {
            var abilitySpec = Instantiate(_VisualScriptingAbilitySpec, abilitySystemComponent.transform);

            abilitySpec.Setup(this, abilitySystemComponent, level);

            return abilitySpec;
        }
    }
}

#endif