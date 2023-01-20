using UnityEngine;
using System.Diagnostics;

namespace StudioScor.AbilitySystem
{
    public abstract class Ability : ScriptableObject
    {
        [SerializeField] protected string _Name;
        [SerializeField] protected Sprite _Icon;
        
        public string Name => _Name;
        public Sprite Icon => _Icon;


        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug;
        public bool UseDebug => _UseDebug;

        #region EDITOR ONLY

        [Conditional("UNITY_EDITOR")]
        protected void Log(object content, bool isError = false)
        {
#if UNITY_EDITOR
            if (isError)
            {
                UnityEngine.Debug.LogError(name + " [ " + GetType().Name + " ] : " + content, this);

                return;
            }

            if (UseDebug)
                UnityEngine.Debug.Log(name + " [ " + GetType().Name + " ] : " + content, this);
#endif
        }
        #endregion

        public virtual bool CanGrantAbility(AbilitySystemComponent abilitySystemComponent)
        {
            return true;
        }

        public abstract IAbilitySpec CreateSpec(AbilitySystemComponent abilitySystemComponent, int level = 0);
    }
}
