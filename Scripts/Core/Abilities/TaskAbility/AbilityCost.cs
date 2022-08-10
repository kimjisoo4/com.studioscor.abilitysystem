using UnityEngine;

namespace KimScor.GameplayTagSystem.Ability
{
    public abstract class AbilityCostSpec
    {
        private AbilityCost _AbilityCost;
        private AbilitySpec _AbilitySpec;

        public AbilityCost AbilityCost => _AbilityCost;
        public AbilitySpec AbilitySpec => _AbilitySpec;

        public AbilityCostSpec(AbilityCost abilityCost, AbilitySpec abilitySpec)
        {
            _AbilityCost = abilityCost;
            _AbilitySpec = abilitySpec;
        }

        public abstract void ConsumeCost();
        public abstract bool CanConsumeCost();
    }

    public abstract class AbilityCost : ScriptableObject
    {
        public abstract AbilityCostSpec CreateSpec(AbilitySpec abilitySpec);
    }
}