using UnityEngine;

#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;
#endif

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

        #region Eanble VisualScripting
#if SCOR_ENABLE_VISUALSCRIPTING
        [Inspectable]
#endif
        #endregion
        public Ability Ability;

        #region Eanble VisualScripting
#if SCOR_ENABLE_VISUALSCRIPTING
        [Inspectable]
#endif
        #endregion
        public int Level;
    }
}
