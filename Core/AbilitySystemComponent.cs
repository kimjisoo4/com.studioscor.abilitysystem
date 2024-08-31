using StudioScor.Utilities;
using System;
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

        public IReadOnlyDictionary<Ability, IAbilitySpec> Abilities { get; }

        public void Tick(float deltaTime);
        public void FixedTick(float deltaTime);

        public (bool isGrant, IAbilitySpec abilitySpec) TryGrantAbility(Ability ability, int level = 0);
        public bool TryRemoveAbility(Ability ability);
        public (bool isActivate, IAbilitySpec abilitySpec) TryActivateAbility(Ability ability);
        public (bool isPossible, IAbilitySpec abilitySpec) CanActivateAbility(Ability ability);
        public IAbilitySpec ForceActivateAbility(Ability ability);
        public void ReleasedAbility(Ability ability);
        public bool IsPlayingAbility(Ability ability);
        public void CancelAbility(Ability ability);
        public void CancelAbilityFromSource(object source);

        public event AbilitySpecHandler OnActivatedAbility;
        public event AbilitySpecHandler OnReleasedAbility;
        public event AbilitySpecHandler OnEndedAbility;
        public event AbilitySpecHandler OnGrantedAbility;
        public event AbilitySpecHandler OnRemovedAbility;
    }
    public static class AbilitySystemUtility
    {
        #region Get Ability System
        public static IAbilitySystem GetAbilitySystem(this GameObject gameObject)
        {
            return gameObject.GetComponent<IAbilitySystem>();
        }
        public static IAbilitySystem GetAbilitySystem(this Component component)
        {
            return component.GetComponent<IAbilitySystem>();
        }
        public static bool TryGetAbilitySystem(this GameObject gameObject, out IAbilitySystem abilitySystem)
        {
            return gameObject.TryGetComponent(out abilitySystem);
        }
        public static bool TryGetAbilitySystem(this Component component, out IAbilitySystem abilitySystem)
        {
            return component.TryGetComponent(out abilitySystem);
        }
        #endregion

        public static bool HasAbility(this IAbilitySystem abilitySystem, Ability ability)
        {
            return abilitySystem.Abilities.ContainsKey(ability);
        }
        public static bool HasAbility(this IAbilitySystem abilitySystem, Type type)
        {
            foreach (var ability in abilitySystem.Abilities.Keys)
            {
                if (ability.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        #region Get Ability Spec
        public static IAbilitySpec GetAbilitySpec(this IAbilitySystem abilitySystem, Ability ability)
        {
            return abilitySystem.Abilities[ability];
        }
        public static IAbilitySpec GetAbilitySpec(this IAbilitySystem abilitySystem, Type type)
        {
            foreach (var ability in abilitySystem.Abilities.Keys)
            {
                if (ability.GetType() == type)
                {
                    return abilitySystem.Abilities[ability];
                }
            }

            return null;
        }

        public static bool TryGetAbilitySpec(this IAbilitySystem abilitySystem, Ability abilitym, out IAbilitySpec spec)
        {
            spec = abilitySystem.GetAbilitySpec(abilitym);

            return spec is not null;
        }

        public static bool TryGetAbilitySpec(this IAbilitySystem abilitySystem, Type type, out IAbilitySpec abilitySpec)
        {
            abilitySpec = abilitySystem.GetAbilitySpec(type);

            return abilitySpec is not null;
        }
        #endregion
    }

    [DefaultExecutionOrder(AbilitySystemExecutionOrder.MAIN_ORDER)]
    [AddComponentMenu("StudioScor/AbilitySystem/AbilitySystem Component", order: 0)]
    public class AbilitySystemComponent : BaseMonoBehaviour, IAbilitySystem
    {
        [Header(" [ Setup] ")]
        [SerializeField] private FAbility[] initAbilities;

        private readonly Dictionary<Ability, IAbilitySpec> _abilities = new();
        private readonly List<IUpdateableAbilitySpec> _updateableAbilitySpecs = new();

        public IReadOnlyDictionary<Ability,IAbilitySpec> Abilities => _abilities;

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

            foreach (var ability in _updateableAbilitySpecs)
            {
                ability.UpdateAbility(deltaTime);
            }
        }

        public override void FixedTick(float deltaTime)
        {
            base.FixedTick(deltaTime);

            foreach (var ability in _updateableAbilitySpecs)
            {
                ability.FixedUpdateAbility(deltaTime);
            }
        }

        public (bool isPossible, IAbilitySpec abilitySpec) CanActivateAbility(Ability ability)
        {
            if(Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                return (spec.CanActiveAbility(), spec);
            }
            else
            {
                return (false, null);
            }
        }
        public bool IsPlayingAbility(Ability ability)
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

        public (bool isActivate, IAbilitySpec abilitySpec) TryActivateAbility(Ability ability)
        {
            if (!ability)
                return (false, null);

            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                return (spec.TryActiveAbility(), spec);
            }
            else
            {
                return (false, null);
            }
        }

        public IAbilitySpec ForceActivateAbility(Ability ability)
        {
            if (!ability)
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

        public void ReleasedAbility(Ability ability)
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
                spec.CancelAbilityFromSource(source);
            }
        }
        public void CancelAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
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
        public (bool isGrant, IAbilitySpec abilitySpec) TryGrantAbility(Ability addAbility, int level = 0)
        {
            if (!CanGrantAbility(addAbility, level))
                return (false, null);

            var abilitySpec = ForceGrantAbility(addAbility, level);

            return (true, abilitySpec);
        }
        public virtual bool CanGrantAbility(Ability ability, int level = 0)
        {
            if (!ability)
            {
                Log(ability + " is Null");
                
                return false;
            }

            return ability.CanGrantAbility(this);
        }
        public IAbilitySpec ForceGrantAbility(Ability ability, int level = 0)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                spec.SetAbilityLevel(level);

                return spec;
            }

            var abilitySpec = ability.CreateSpec(this, level);

            abilitySpec.OnActivatedAbility += Callback_OnActivatedAbility;
            abilitySpec.OnReleasedAbility += Callback_OnReleasedAbility;
            abilitySpec.OnEndedAbility += Callback_OnEndedAbility;

            abilitySpec.GrantAbility();

            _abilities.Add(ability, abilitySpec);

            if(abilitySpec is IUpdateableAbilitySpec updateableAbility)
            {
                _updateableAbilitySpecs.Add(updateableAbility);
            }

            Callback_OnGrantedAbility(abilitySpec);

            return abilitySpec;
        }

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

            if (!Abilities.ContainsKey(ability))
            {
                Log("Has Not " + ability);

                return false;
            }

            return true;

        }
        public void ForceRemoveAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                spec.CancelAbility();
                spec.RemoveAbility();

                spec.OnActivatedAbility -= Callback_OnActivatedAbility;
                spec.OnReleasedAbility -= Callback_OnReleasedAbility;
                spec.OnEndedAbility -= Callback_OnEndedAbility;

                _abilities.Remove(ability);

                if(spec is IUpdateableAbilitySpec updateableAbility)
                {
                    _updateableAbilitySpecs.Remove(updateableAbility);
                }

                Callback_OnRemovedAbility(spec);
            }
        }

        public void RemoveAllAbility()
        {
            if (_abilities is null || _abilities.Count <= 0)
                return;

            var abilities = _abilities.Keys;

            for(int i = abilities.LastIndex(); i >= 0; i--)
            {
                var ability = abilities.ElementAtOrDefault(i);

                if(ability)
                    ForceRemoveAbility(ability);
            }

            _abilities.Clear();
        }


        #region CallBack
        protected virtual void Callback_OnGrantedAbility(IAbilitySpec grantAbilitySpec)
        {
            Log("On Added Ability - " + grantAbilitySpec.Ability.ID);

            OnGrantedAbility?.Invoke(this, grantAbilitySpec);
        }
        protected virtual void Callback_OnRemovedAbility(IAbilitySpec removeAbilitySpec)
        {
            Log("On Removed Ability - " + removeAbilitySpec.Ability.ID);

            OnRemovedAbility?.Invoke(this, removeAbilitySpec);
        }

        protected virtual void Callback_OnActivatedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Activated Ability - " + abilitySpec.Ability.ID);

            OnActivatedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void Callback_OnReleasedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Released Ability - " + abilitySpec.Ability.ID);

            OnReleasedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void Callback_OnEndedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Ended Ability - " + abilitySpec.Ability.ID);

            OnEndedAbility?.Invoke(this, abilitySpec);
        }

        #endregion
    }
}
