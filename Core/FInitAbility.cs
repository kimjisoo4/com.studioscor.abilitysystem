using UnityEngine;

namespace StudioScor.AbilitySystem
{
    [System.Serializable]
    public struct FAbility
    {
        public FAbility(Ability ability, int level)
        {
            Ability = ability;
            Level = level;
        }

        public Ability Ability;
        public int Level;
    }
}
