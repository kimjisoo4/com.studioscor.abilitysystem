using StudioScor.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace StudioScor.AbilitySystem
{

    public interface IAbilitySystem
    {
        public delegate void AbilitySpecHandler(IAbilitySystem abilitySystem, IAbilitySpec abilitySpec);
        public GameObject gameObject { get; }
        public Transform transform { get; }

        public IReadOnlyDictionary<IAbility, IAbilitySpec> Abilities { get; }

        public void Tick(float deltaTime);
        public void FixedTick(float deltaTime);

        public bool TryGrantAbility(IAbility ability, int level = 0);
        public bool TryGrantAbility(IAbility ability, int level, out IAbilitySpec abilitySpec);

        public bool RemoveAbility(IAbility ability);
        
        public bool TryActivateAbility(IAbility ability);
        public bool TryActivateAbility(IAbility ability, out IAbilitySpec abilitySpec);
        public bool CanActivateAbility(IAbility ability); 
        public bool CanActivateAbility(IAbility ability, out IAbilitySpec abilitySpec);
        public IAbilitySpec ForceActivateAbility(IAbility ability);
        
        public void ReleasedAbility(IAbility ability);
        public bool IsPlayingAbility(IAbility ability);
        
        public void CancelAbility(IAbility ability);
        public void CancelAbilityFromSource(object source);

        public event AbilitySpecHandler OnActivatedAbility;
        public event AbilitySpecHandler OnReleasedAbility;
        public event AbilitySpecHandler OnEndedAbility;
        public event AbilitySpecHandler OnGrantedAbility;
        public event AbilitySpecHandler OnRemovedAbility;
    }

    [DefaultExecutionOrder(AbilitySystemExecutionOrder.MAIN_ORDER)]
    [AddComponentMenu("StudioScor/AbilitySystem/AbilitySystem Component", order: 0)]
    public class AbilitySystemComponent : BaseMonoBehaviour, IAbilitySystem
    {
        [Header(" [ Setup] ")]
        [SerializeField] private FAbility[] initAbilities;

        private readonly Dictionary<IAbility, IAbilitySpec> _abilities = new();
        private readonly List<IUpdateableAbilitySpec> _updateableAbilitySpecs = new();
        private readonly Queue<IUpdateableAbilitySpec> _removalAbilities = new();
        public IReadOnlyDictionary<IAbility, IAbilitySpec> Abilities => _abilities;

        public event IAbilitySystem.AbilitySpecHandler OnActivatedAbility;
        public event IAbilitySystem.AbilitySpecHandler OnReleasedAbility;
        public event IAbilitySystem.AbilitySpecHandler OnEndedAbility;

        public event IAbilitySystem.AbilitySpecHandler OnGrantedAbility;
        public event IAbilitySystem.AbilitySpecHandler OnRemovedAbility;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (initAbilities is null || initAbilities.Length == 0)
                return;

            for(int i = 0; i < initAbilities.Length; i++)
            {
                if(initAbilities[i].Ability)
                    initAbilities[i].AbilityName = $"{initAbilities[i].Ability.name} [ Level : {initAbilities[i].Level} ]";
            }
#endif
        }

        protected virtual void Awake()
        {
            Setup();
        }
        protected virtual void OnDestroy()
        {
            RemoveAllAbility();
        }

        protected void Setup()
        {
            Log("Setup Ability System");

            foreach (var ability in initAbilities)
            {
                TryGrantAbility(ability.Ability, ability.Level);
            }
        }

        public void ResetAbilitySystem()
        {
            Log("Reset Ability System");

            RemoveAllAbility();

            foreach (var ability in initAbilities)
            {
                TryGrantAbility(ability.Ability, ability.Level);
            }
        }

        protected virtual void OnSetup() { }
        protected virtual void OnReset() { }


        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            for(int i = 0; i < _updateableAbilitySpecs.Count; i++)
            {
                var ability = _updateableAbilitySpecs[i];

                ability.UpdateAbility(deltaTime);
            }

            RemovalQueue();
        }

        public override void FixedTick(float deltaTime)
        {
            base.FixedTick(deltaTime);

            for (int i = 0; i < _updateableAbilitySpecs.Count; i++)
            {
                var ability = _updateableAbilitySpecs[i];

                ability.FixedUpdateAbility(deltaTime);
            }

            RemovalQueue();
        }

        private void RemovalQueue()
        {
            if (_removalAbilities.Count > 0)
            {
                foreach (var removeAbility in _removalAbilities)
                {
                    _updateableAbilitySpecs.Remove(removeAbility);
                }

                _removalAbilities.Clear();
            }
        }

        public bool CanActivateAbility(IAbility ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec abilitySpec))
            {
                return abilitySpec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }

        public bool CanActivateAbility(IAbility ability, out IAbilitySpec abilitySpec)
        {
            if (Abilities.TryGetValue(ability, out abilitySpec))
            {
                return abilitySpec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public bool IsPlayingAbility(IAbility ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                return spec.IsPlaying;
            }
            else
            {
                return false;
            }
        }

        public bool TryActivateAbility(IAbility ability)
        {
            if (ability is null)
                return false;

            if (Abilities.TryGetValue(ability, out IAbilitySpec abilitySpec))
            {
                return abilitySpec.TryActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public bool TryActivateAbility(IAbility ability, out IAbilitySpec abilitySpec)
        {
            if (ability is null)
            {
                abilitySpec = null;
                return false;
            }


            if (Abilities.TryGetValue(ability, out abilitySpec))
            {
                return abilitySpec.TryActiveAbility();
            }
            else
            {
                return false;
            }
        }

        public IAbilitySpec ForceActivateAbility(IAbility ability)
        {
            if (ability is null)
                return null;

            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                spec.ForceActiveAbility();
                return spec;
            }
            else
            {
                return null;
            }
        }

        public void ReleasedAbility(IAbility ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                if(spec.IsPlaying)
                {
                    spec.ReleaseAbility();
                }
            }
        }

        public void CancelAbilityFromSource(object source)
        {
            foreach (var spec in Abilities.Values)
            {
                if (!spec.IsPlaying)
                    continue;

                spec.CancelAbilityFromSource(source);
            }
        }
        public void CancelAbility(IAbility ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                if (!spec.IsPlaying)
                    return;

                spec.CancelAbility();
            }
        }
        public void CancelAllAbility()
        {
            foreach (var spec in Abilities.Values)
            {
                spec.CancelAbility();
            }
        }
        public bool TryGrantAbility(IAbility addAbility, int level = 0)
        {
            if (!CanGrantAbility(addAbility, level))
            {
                return false;
            }

            ForceGrantAbility(addAbility, level);

            return true;
        }
        public bool TryGrantAbility(IAbility addAbility, int level, out IAbilitySpec abilitySpec)
        {
            if (!CanGrantAbility(addAbility, level))
            {
                abilitySpec = null;
                return false;
            }

            abilitySpec = ForceGrantAbility(addAbility, level);

            return true;
        }

        public virtual bool CanGrantAbility(IAbility ability, int level = 0)
        {
            if (ability is null)
            {
                Log(ability + " is Null");
                
                return false;
            }

            return ability.CanGrantAbility(this);
        }
        public IAbilitySpec ForceGrantAbility(IAbility ability, int level = 0)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                spec.SetAbilityLevel(level);

                return spec;
            }

            var abilitySpec = ability.CreateSpec(this, level);

            abilitySpec.OnActivatedAbility += Invoke_OnActivatedAbility;
            abilitySpec.OnReleasedAbility += Invoke_OnReleasedAbility;
            abilitySpec.OnEndedAbility += Invoke_OnEndedAbility;

            abilitySpec.GrantAbility();

            _abilities.Add(ability, abilitySpec);

            if(abilitySpec is IUpdateableAbilitySpec updateableAbility)
            {
                _updateableAbilitySpecs.Add(updateableAbility);
            }

            Invoke_OnGrantedAbility(abilitySpec);

            return abilitySpec;
        }

        public bool RemoveAbility(IAbility ability)
        {
            if (ability is null)
                return false;

            if (!Abilities.TryGetValue(ability, out IAbilitySpec spec))
                return false;

            spec.CancelAbility();
            spec.RemoveAbility();

            spec.OnActivatedAbility -= Invoke_OnActivatedAbility;
            spec.OnReleasedAbility -= Invoke_OnReleasedAbility;
            spec.OnEndedAbility -= Invoke_OnEndedAbility;

            _abilities.Remove(ability);

            if (spec is IUpdateableAbilitySpec updateableAbility)
            {
                _removalAbilities.Enqueue(updateableAbility);
            }

            Invoke_OnRemovedAbility(spec);

            return true;
        }

        public void RemoveAllAbility()
        {
            if (_abilities is null || _abilities.Count <= 0)
                return;

            var abilities = _abilities.Keys;

            for(int i = abilities.LastIndex(); i >= 0; i--)
            {
                var ability = abilities.ElementAtOrDefault(i);

                if(ability is not null)
                    RemoveAbility(ability);
            }

            _abilities.Clear();
        }


        #region Invoke
        private void Invoke_OnGrantedAbility(IAbilitySpec abilitySpec)
        {
            Log($"{nameof(OnGrantedAbility)} :: {abilitySpec.Ability.ID}");

            OnGrantedAbility?.Invoke(this, abilitySpec);
        }
        private void Invoke_OnRemovedAbility(IAbilitySpec abilitySpec)
        {
            Log($"{nameof(OnRemovedAbility)} :: {abilitySpec.Ability.ID}");

            OnRemovedAbility?.Invoke(this, abilitySpec);
        }

        private void Invoke_OnActivatedAbility(IAbilitySpec abilitySpec)
        {
            Log($"{nameof(OnActivatedAbility)} :: {abilitySpec.Ability.ID}");

            OnActivatedAbility?.Invoke(this, abilitySpec);
        }
        private void Invoke_OnReleasedAbility(IAbilitySpec abilitySpec)
        {
            Log($"{nameof(OnReleasedAbility)} :: {abilitySpec.Ability.ID}");

            OnReleasedAbility?.Invoke(this, abilitySpec);
        }
        private void Invoke_OnEndedAbility(IAbilitySpec abilitySpec)
        {
            Log($"{nameof(OnEndedAbility)} :: {abilitySpec.Ability.ID}");

            OnEndedAbility?.Invoke(this, abilitySpec);
        }

        #endregion
    }
}
