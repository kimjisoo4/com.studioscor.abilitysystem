using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace KimScor.GameplayTagSystem.Ability
{
    public class AbilitySystem : MonoBehaviour
    {
        #region Events
        public delegate void UpdateAbilityHandler(AbilitySystem abilitySystem, float deltaTime);
        public delegate void AbilityChangedHandler(AbilitySystem abilitySystem, AbilitySpec abilitySpec);
        public delegate void CancelAbilityHandler(AbilitySystem abilitySystem, GameplayTag[] cancelTags);
        #endregion



        [SerializeField] private GameplayTagSystem _GameplayTagSystem;

        public GameplayTagSystem GameplayTagSystem
        {
            get
            {
                if (_GameplayTagSystem == null)
                {
                    TryGetComponent(out _GameplayTagSystem);
                }

                return _GameplayTagSystem;
            }
        }

        private Dictionary<Ability, AbilitySpec> _Abilities;
        public IReadOnlyDictionary<Ability, AbilitySpec> Abilities
        {
            get
            {
                if (_Abilities == null)
                {
                    _Abilities = new Dictionary<Ability, AbilitySpec>();
                }

                return _Abilities;
            }
        }

        private AbilityInputBuffer _AbilityInputBuffer;

        public AbilityInputBuffer InputBuffer
        {
            get
            {
                if (_AbilityInputBuffer == null)
                {
                    _AbilityInputBuffer = new AbilityInputBuffer();
                }

                return _AbilityInputBuffer;
            }
        }

        [SerializeField] private float _BufferDuration = 0.2f;
        [SerializeField] private Ability[] _InitializationAbilities;

        public bool DebugMode = false;

        public event AbilityChangedHandler OnAddedAbility;
        public event AbilityChangedHandler OnRemovedAbility;



#if UNITY_EDITOR
        private void Reset()
        {
            TryGetComponent(out _GameplayTagSystem);
        }
#endif

        

        #region CallBack
        public void OnAddAbility(AbilitySpec addAbilitySpec)
        {
            OnAddedAbility?.Invoke(this, addAbilitySpec);
        }
        public void OnRemoveAbility(AbilitySpec removeAbilitySpec)
        {
            OnRemovedAbility?.Invoke(this, removeAbilitySpec);
        }
        #endregion

        private void Awake()
        {
            if (_Abilities == null)
            {
                _Abilities = new Dictionary<Ability, AbilitySpec>();
            }

            foreach (Ability ability in _InitializationAbilities)
            {
                AddNewAbility(ability, 0);
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            InputBuffer.Buffer(deltaTime);

            foreach (AbilitySpec spec in Abilities.Values)
            {
                spec.OnUpdateAbility(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            foreach (AbilitySpec spec in Abilities.Values)
            {
                spec.OnFixedUpdateAbility(deltaTime);
            }
        }
        public void OnCancelAbility(GameplayTag[] cancelTags)
        {
            foreach (AbilitySpec spec in Abilities.Values)
            {
                spec.TryCancelAbility(cancelTags);
            }
        }


        public void OverrideAbilities(AbilitySystem abilitySystem)
        {
            foreach (AbilitySpec spec in abilitySystem.Abilities.Values)
            {
                AddNewAbility(spec.Ability, spec.Level);
            }
        }

        #region Add Remove
        /// <summary>
        /// 새로운 어빌리티를 추가함.
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="level"></param>
        public AbilitySpec AddNewAbility(Ability ability, int level = 1)
        {
            if (DebugMode)
                Debug.Log("Add New Ability : " + ability.name);


            if (Abilities.TryGetValue(ability, out AbilitySpec containSpec))
            {
                // TODO 가지고 있는 스킬에 특정 이벤트를 실행해야함 ... 경험치 증가, 효과 변경 등 ... //

                return containSpec;
            }

            var spec = ability.CreateSpec(this, level);

            _Abilities.Add(ability, spec);

            spec.OnGrantAbility();

            OnAddAbility(spec);

            return spec;
        }

        /// <summary>
        /// 보유한 특정 어빌리티를 제거함
        /// </summary>
        /// <param name="ability"></param>
        public void RemoveAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                _Abilities.Remove(ability);

                spec.OnLostAbility();

                OnRemoveAbility(spec);
            }
        }

        #endregion

        public void TryActivateAbilityWithInputBuffer(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                if (!spec.TryActivateAbility())
                {
                    InputBuffer.SetBuffer(spec, _BufferDuration);
                }
            }
        }

        public void OnReleasedAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                spec.OnReleasedAbility();
            }
        }

        /// <summary>
        /// 해당 어빌리티를 보유하고 있는가.
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CheckHasAbility(Ability ability)
        {
            return Abilities.ContainsKey(ability);
        }

        /// <summary>
        /// 해당 어빌리티를 사용할 수 있는가
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CheckCanActivateAbility(Ability ability)
        {
            if(Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 해당 어빌리티가 사용되고 있는가
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool CheckActivatingAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                return spec.Activate;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 해당 어빌리티 사용을 시도함
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        public bool TryActivateAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                return spec.TryActivateAbility();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 어빌리티를 태그로 검색하여 시도함.
        /// </summary>
        /// <param name="abilityTag"></param>
        /// <returns></returns>
        public bool TryActivateAbilityWithTag(GameplayTag abilityTag)
        {
            foreach (AbilitySpec spec in Abilities.Values)
            {
                if (abilityTag == spec.Ability.AbilityTag)
                {
                    bool activate = spec.TryActivateAbility();

                    return activate;
                }
            }

            return false;
        }

        public void ActivateAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                spec.OnAbility();
            }
        }
        public bool TryGetAbility(Ability ability, out AbilitySpec abilitySpec)
        {
            if (Abilities.TryGetValue(ability, out AbilitySpec spec))
            {
                abilitySpec = spec;

                return true;
            }
            else
            {
                abilitySpec = null;

                return false;
            }
        }

        public bool TryFindAbility(Type type, out AbilitySpec abilitySpec) 
        {
            foreach (var ability in Abilities.Keys)
            {
                if(ability.GetType().Equals(type))
                {
                    return Abilities.TryGetValue(ability, out abilitySpec);
                }
            }

            abilitySpec = null;

            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (AbilitySpec spec in Abilities.Values)
            {
                spec.OnDrawGizmos();
            }
        }
#endif
    }
}
