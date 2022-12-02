using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;

namespace KimScor.GameplayTagSystem.Ability
{
    public class AbilitySystem : MonoBehaviour
    {
        #region Events
        public delegate void AbilityChangedHandler(AbilitySystem abilitySystem, IAbilitySpec abilitySpec);
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

        private List<IAbilitySpec> _Abilities;
        public IReadOnlyList<IAbilitySpec> Abilities
        {
            get
            {
                if (_Abilities == null)
                {
                    _Abilities = new();
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

        private bool _WasSetup = false;

        public event AbilityChangedHandler OnStartedAbility;
        public event AbilityChangedHandler OnFinishedAbility;

        public event AbilityChangedHandler OnAddedAbility;
        public event AbilityChangedHandler OnRemovedAbility;


#if UNITY_EDITOR
        private void Reset()
        {
            TryGetComponent(out _GameplayTagSystem);
        }
#endif

        #region EDITOR ONLY
#if UNITY_EDITOR
        [SerializeField] private bool _UseDebug;

        public bool UseDebug => _UseDebug;
#endif

        [Conditional("UNITY_EDITOR")]
        protected void Log(object content)
        {
#if UNITY_EDITOR
            if (UseDebug)
                UnityEngine.Debug.Log("Ability Sytstem [ " + transform.name + " ] : " + content, this);
#endif
        }
        #endregion

        private void Awake()
        {
            if (!_WasSetup)
                Setup();
        }

        protected virtual void Setup()
        {
            _WasSetup = true;

            Log("Setup");

            _Abilities = new();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            InputBuffer.Buffer(deltaTime);

            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].OnUpdateAbility(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].OnFixedUpdateAbility(deltaTime);
            }
        }

        #region Get Ability Spec
        public bool TryGetAbilitySpec(Ability abilitym, out IAbilitySpec spec)
        {
            spec = GetAbilitySpec(abilitym);

            return spec is not null;
        }
        public IAbilitySpec GetAbilitySpec(Ability ability)
        {
            foreach (var spec in Abilities)
            {
                if (spec.Ability == ability)
                {
                    return spec;
                }
            }

            return null;
        }
        public bool TryGetAbilityWithType(Type type, out IAbilitySpec abilitySpec)
        {
            abilitySpec = GetAbilitySpecWithType(type);

            return abilitySpec is not null;
        }
        public IAbilitySpec GetAbilitySpecWithType(Type type)
        {
            foreach (var ability in Abilities)
            {
                if (ability.Ability.GetType() == type)
                {
                    return ability;
                }
            }

            return null;
        }

        #endregion

        #region Get Ability State 
        public bool HasAbility(Ability ability)
        {
            return GetAbilitySpec(ability) is not null;
        }

        public int GetAbilityLevel(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                return spec.Level;
            }

            return -1;
        }
        public bool CanActivateAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public bool IsPlayingAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                return spec.IsPlaying;
            }
            else
            {
                return false;
            }
        }
        public bool IsPlayingAbilityWithType(Type type)
        {
            if (TryGetAbilityWithType(type, out var abilitySpec))
            {
                return abilitySpec.IsPlaying;
            }

            return false;
        }
        #endregion

        #region Ability Control
        public void TryActivateAbilityWithInputBuffer(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                if (!spec.TryActiveAbility())
                {
                    InputBuffer.SetBuffer(spec, _BufferDuration);
                }
            }
        }
        public bool TryActivateAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                return spec.TryActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public bool TryActivateAbilityWithTag(GameplayTag abilityTag)
        {
            foreach (IAbilitySpec spec in Abilities)
            {
                if (abilityTag == spec.Ability.Tag)
                {
                    bool activate = spec.TryActiveAbility();

                    return activate;
                }
            }

            return false;
        }

        public void ActivateAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                spec.ForceActiveAbility();
            }
        }
        public void OnReleasedAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                spec.OnReleaseAbility();
            }
        }
        public void OnCancelAbility(GameplayTag[] cancelTags)
        {
            foreach (IAbilitySpec spec in Abilities)
            {
                spec.TryCancelAbility(cancelTags);
            }
        }
        public void OnCancelAbility(IReadOnlyCollection<GameplayTag> cancelTags)
        {
            GameplayTag[] tags = cancelTags.ToArray();

            foreach (IAbilitySpec spec in Abilities)
            {
                spec.TryCancelAbility(tags);
            }
        }
        public void OnCancelAllAbility()
        {
            foreach (var spec in Abilities)
            {
                spec.ForceCancelAbility();
            }
        }
        #endregion

        #region Add Remove
        public bool TryAddAbility(Ability addAbility, int level = 1)
        {
            if (!addAbility)
                return false;

            Log("Add New Ability : " + addAbility.name);

            if (TryGetAbilitySpec(addAbility, out IAbilitySpec spec))
            {
                // TODO 가지고 있는 스킬에 특정 이벤트를 실행해야함 ... 경험치 증가, 효과 변경 등 ... //

                return false;
            }

            var newAbilitySpec = addAbility.CreateSpec(this, level);

            _Abilities.Add(newAbilitySpec);

            newAbilitySpec.OnActivatedAbility += Spec_OnStartedAbility;
            newAbilitySpec.OnFinishedAbility += Spec_OnFinishedAbility;

            newAbilitySpec.OnGrantAbility();

            OnAddAbility(newAbilitySpec);

            return true;
        }
        public bool TryRemoveAbility(Ability ability)
        {
            if (!TryGetAbilitySpec(ability, out IAbilitySpec spec))
                return false;

            _Abilities.Remove(spec);

            spec.OnActivatedAbility -= Spec_OnStartedAbility;
            spec.OnFinishedAbility -= Spec_OnFinishedAbility;

            spec.OnLostAbility();

            OnRemoveAbility(spec);

            return true;
        }
        #endregion
        


        private void Spec_OnFinishedAbility(IAbilitySpec abilitySpec)
        {
            OnFinishAbility(abilitySpec);
        }
        private void Spec_OnStartedAbility(IAbilitySpec abilitySpec)
        {
            OnStartAbility(abilitySpec);
        }

        #region CallBack
        protected virtual void OnAddAbility(IAbilitySpec addAbilitySpec)
        {
            Log("On Added Ability - " + addAbilitySpec);

            OnAddedAbility?.Invoke(this, addAbilitySpec);
        }
        protected virtual void OnRemoveAbility(IAbilitySpec removeAbilitySpec)
        {
            Log("On Removed Ability - " + removeAbilitySpec);

            OnRemovedAbility?.Invoke(this, removeAbilitySpec);
        }
        protected virtual void OnFinishAbility(IAbilitySpec abilitySpec)
        {
            Log("On Finished Ability - " + abilitySpec);

            OnFinishedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void OnStartAbility(IAbilitySpec abilitySpec)
        {
            Log("On Started Ability - " + abilitySpec);

            OnStartedAbility?.Invoke(this, abilitySpec);
        }
        #endregion
    }
}
