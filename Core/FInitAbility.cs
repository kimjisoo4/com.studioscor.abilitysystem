using UnityEngine;

namespace StudioScor.AbilitySystem
{
    [System.Serializable]
    public struct FInitAbility
    {
        public FInitAbility(Ability ability, int level)
        {
            Ability = ability;
            Level = level;
        }

        public Ability Ability;
        public int Level;
    }
}
