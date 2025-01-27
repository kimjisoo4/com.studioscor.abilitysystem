using System;
using UnityEngine;


namespace StudioScor.AbilitySystem
{
    public static class AbilitySystemUtility
    {
        #region Get Ability System
        public static IAbilitySystem GetAbilitySystem(this GameObject gameObject)
        {
            return gameObject.GetComponent<IAbilitySystem>();
        }
        public static IAbilitySystem GetAbilitySystem(this Component component)
        {
            return component.GetComponent<IAbilitySystem>();
        }
        public static bool TryGetAbilitySystem(this GameObject gameObject, out IAbilitySystem abilitySystem)
        {
            return gameObject.TryGetComponent(out abilitySystem);
        }
        public static bool TryGetAbilitySystem(this Component component, out IAbilitySystem abilitySystem)
        {
            return component.TryGetComponent(out abilitySystem);
        }
        #endregion

        public static bool HasAbility(this IAbilitySystem abilitySystem, IAbility ability)
        {
            return abilitySystem.Abilities.ContainsKey(ability);
        }
        public static bool HasAbility(this IAbilitySystem abilitySystem, Type type)
        {
            foreach (var ability in abilitySystem.Abilities.Keys)
            {
                if (ability.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        #region Get Ability Spec
        public static IAbilitySpec GetAbilitySpec(this IAbilitySystem abilitySystem, IAbility ability)
        {
            if(abilitySystem.Abilities.ContainsKey(ability))
                return abilitySystem.Abilities[ability];
            
            return null;
        }
        public static IAbilitySpec GetAbilitySpec(this IAbilitySystem abilitySystem, Type type)
        {
            foreach (var ability in abilitySystem.Abilities.Keys)
            {
                if (ability.GetType() == type)
                {
                    return abilitySystem.Abilities[ability];
                }
            }

            return null;
        }

        public static bool TryGetAbilitySpec(this IAbilitySystem abilitySystem, IAbility ability, out IAbilitySpec spec)
        {
            if(ability is null)
            {
                spec = null;
                return false;
            }

            return abilitySystem.Abilities.TryGetValue(ability, out spec);
        }

        public static bool TryGetAbilitySpec(this IAbilitySystem abilitySystem, Type type, out IAbilitySpec abilitySpec)
        {
            abilitySpec = abilitySystem.GetAbilitySpec(type);

            return abilitySpec is not null;
        }
        #endregion
    }
}
