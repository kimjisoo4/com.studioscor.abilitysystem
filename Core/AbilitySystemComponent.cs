using System.Collections.Generic;
using UnityEngine;
using System;
using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{
    [DefaultExecutionOrder(AbilitySystemExecutionOrder.MAIN_ORDER)]
    [AddComponentMenu("StudioScor/AbilitySystem/AbilitySystem Component", order: 0)]
    public class AbilitySystemComponent : BaseMonoBehaviour
    {
        #region Events
        public delegate void AbilityChangedHandler(AbilitySystemComponent abilitySystemComponent, IAbilitySpec abilitySpec);
        #endregion

        [Header(" [ Setup] ")]
        [SerializeField] private FInitAbility[] _InitAbilities;
        [SerializeField] private float _BufferDuration = 0.2f;

        [Header(" [ Use Debug ] ")]
        [SerializeField] private bool _UseDebug;

        private List<IAbilitySpec> _Abilities;
        private AbilityInputBuffer _AbilityInputBuffer;

        public IReadOnlyList<IAbilitySpec> Abilities => _Abilities;

        public event AbilityChangedHandler OnActivatedAbility;
        public event AbilityChangedHandler OnFinishedAbility;

        public event AbilityChangedHandler OnGrantedAbility;
        public event AbilityChangedHandler OnRemovedAbility;

        private void Awake()
        {
            Setup();
        }

        private void Start()
        {
            foreach (var ability in _InitAbilities)
            {
                TryGrantAbility(ability.Ability, ability.Level);
            }
        }
        protected void Setup()
        {
            Log("Setup");

            _Abilities = new();
            _AbilityInputBuffer = new();
        }

        public void ResetAbilitySystem()
        {
            RemoveAllAbility();

            _AbilityInputBuffer.ResetAbilityInputBuffer();

            foreach (var ability in _InitAbilities)
            {
                TryGrantAbility(ability.Ability, ability.Level);
            }
        }

        protected virtual void OnSetup() { }
        protected virtual void OnReset() { }


        private void Update()
        {
            float deltaTime = Time.deltaTime;

            _AbilityInputBuffer.Buffer(deltaTime);

            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].UpdateAbility(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            for (int i = 0; i < Abilities.Count; i++)
            {
                Abilities[i].FixedUpdateAbility(deltaTime);
            }
        }

        #region Get Ability Spec
        public bool TryGetAbilitySpec(Ability abilitym, out IAbilitySpec spec)
        {
            spec = GetAbilitySpec(abilitym);

            return spec is not null;
        }
        public bool TryGetAbilitySpec(Type type, out IAbilitySpec abilitySpec)
        {
            abilitySpec = GetAbilitySpec(type);

            return abilitySpec is not null;
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
        public IAbilitySpec GetAbilitySpec(Type type)
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
        #region Has Ability Spec
        public bool HasAbility(Ability ability)
        {
            return GetAbilitySpec(ability) is not null;
        }
        
        public bool HasAbility(Type abilityType)
        {
            return GetAbilitySpec(abilityType) is not null;
        }
        #endregion
        #region Can Activate Ability

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
        
        public bool CanActivateAbility(Type abilityType)
        {
            if (TryGetAbilitySpec(abilityType, out IAbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region Is Activate Ability
        public bool IsActivateAbility(Ability ability)
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
        
        public bool IsActivateAbility(Type type)
        {
            if (TryGetAbilitySpec(type, out var abilitySpec))
            {
                return abilitySpec.IsPlaying;
            }

            return false;
        }
        #endregion
        #region Activate Ability Spec
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
        public void TryActivateAbilityAsInputBuffer(Ability ability)
        {
            if (!ability)
                return;

            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                if (!spec.TryActiveAbility())
                {
                    _AbilityInputBuffer.SetBuffer(spec, _BufferDuration);
                }
            }
        }
        

        public void ForceActivateAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                spec.ForceActiveAbility();
            }
        }
        #endregion
        #region Released Ability Spec
        public void ReleasedAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                if(spec.IsPlaying)
                    spec.ReleaseAbility();
            }
        }
        #endregion
        #region Canceled Ability Spec
        
        public void CancelAbilityFromSource(object source)
        {
            foreach (var spec in Abilities)
            {
                spec.CancelAbilityFromSource(source);
            }
        }
        public void CancelAbility(Ability ability)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                spec.ForceCancelAbility();
            }
        }
        public void CancelAllAbility()
        {
            foreach (var spec in Abilities)
            {
                spec.ForceCancelAbility();
            }
        }
        #endregion

        #region Add Ability
        public bool TryGrantAbility(Ability addAbility, int level = 0)
        {
            if (!CanGrantAbility(addAbility, level))
                return false;

            ForceGrantAbility(addAbility, level);

            return true;
        }
        public virtual bool CanGrantAbility(Ability ability, int level = 0)
        {
            if (!ability)
            {
                Log(ability + " is Null");
                
                return false;
            }

            return true;
        }
        public void ForceGrantAbility(Ability ability, int level = 0)
        {
            if (TryGetAbilitySpec(ability, out IAbilitySpec spec))
            {
                spec.OnOverride(level);

                return;
            }

            var newAbilitySpec = ability.CreateSpec(this, level);

            newAbilitySpec.GrantAbility();

            _Abilities.Add(newAbilitySpec);

            newAbilitySpec.OnActivatedAbility += Spec_OnActivatedAbility;
            newAbilitySpec.OnFinishedAbility += Spec_OnFinishedAbility;

            Callback_OnGrantedAbility(newAbilitySpec);
        }
        #endregion
        #region Remove Ability
        public bool TryRemoveAbility(Ability ability)
        {
            if (!CanRemoveAbility(ability))
                return false;

            ForceRemoveAbility(ability);

            return true;
        }

        public virtual bool CanRemoveAbility(Ability ability)
        {
            if (!ability)
            {
                Log(ability + "is Null");

                return false;
            }

            if (!HasAbility(ability))
            {
                Log("Has Not " + ability);

                return false;
            }

            return true;

        }
        public void ForceRemoveAbility(Ability ability)
        {
            var spec = GetAbilitySpec(ability);

            RemoveAbility(spec);
        }
        private void RemoveAbility(IAbilitySpec abilitySpec)
        {
            if (abilitySpec.IsPlaying)
            {
                abilitySpec.ForceCancelAbility();
            }

            _Abilities.Remove(abilitySpec);

            abilitySpec.OnActivatedAbility -= Spec_OnActivatedAbility;
            abilitySpec.OnFinishedAbility -= Spec_OnFinishedAbility;

            abilitySpec.RemoveAbility();

            Callback_OnRemovedAbility(abilitySpec);
        }
        public void RemoveAllAbility()
        {
            for(int i = Abilities.Count - 1; i >= 0; i--)
            {
                RemoveAbility(Abilities[i]);
            }

            _Abilities.Clear();
        }

        #endregion

        #region Ability Spec Delegate
        private void Spec_OnFinishedAbility(IAbilitySpec abilitySpec)
        {
            Callback_OnFinishedAbility(abilitySpec);
        }
        private void Spec_OnActivatedAbility(IAbilitySpec abilitySpec)
        {
            Callback_OnActivatedAbility(abilitySpec);
        }
        #endregion

        #region CallBack
        protected virtual void Callback_OnGrantedAbility(IAbilitySpec grantAbilitySpec)
        {
            Log("On Added Ability - " + grantAbilitySpec.Ability.Name);

            OnGrantedAbility?.Invoke(this, grantAbilitySpec);
        }
        protected virtual void Callback_OnRemovedAbility(IAbilitySpec removeAbilitySpec)
        {
            Log("On Removed Ability - " + removeAbilitySpec.Ability.Name);

            OnRemovedAbility?.Invoke(this, removeAbilitySpec);
        }
        protected virtual void Callback_OnFinishedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Finished Ability - " + abilitySpec.Ability.Name);

            OnFinishedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void Callback_OnActivatedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Activated Ability - " + abilitySpec.Ability.Name);

            OnActivatedAbility?.Invoke(this, abilitySpec);
        }
        #endregion
    }
}
