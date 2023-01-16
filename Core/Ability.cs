using UnityEngine;
using System.Diagnostics;

namespace StudioScor.AbilitySystem
{
    public abstract partial class Ability : ScriptableObject
    {
        [SerializeField] protected string _Name;
        [SerializeField] protected Sprite _Icon;
        
        public string Name => _Name;
        public Sprite Icon => _Icon;


        [Header(" 디버그 ")]
        [SerializeField] private bool _DebugMode;
        public bool UseDebug => _DebugMode;


        #region EDITOR ONLY

        [Conditional("UNITY_EDITOR")]
        protected void Log(object massage)
        {
#if UNITY_EDITOR
            if (UseDebug)
                UnityEngine.Debug.Log(name + " [ " + GetType().Name + " ] : " + massage, this);
#endif
        }
        #endregion

        public abstract IAbilitySpec CreateSpec(AbilitySystem abilitySystem, int level = 0);

        public virtual bool CanGrantAbility(AbilitySystem abilitySystem)
        {
            return true;
        }
    }
}
