using System.Collections.Generic;
using UnityEngine;
using System;
using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{
    public delegate void AbilitySpecHandler(IAbilitySystemEvent abilitySystemEvent, IAbilitySpec abilitySpec);
    public delegate void AbilitySpecEventHandler(IAbilitySystemEvent abilitySystemEvent, IAbilitySpecEvent abilitySpecEvent);

    public interface IAbilitySystem
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }

        public IReadOnlyDictionary<Ability, IAbilitySpec> Abilities { get; }
        public bool TryGrantAbility(Ability ability, int level = 0);
        public bool TryActivateAbility(Ability ability);
        public void ReleasedAbility(Ability ability);
        public bool IsActivateAbility(Ability ability);
        public void CancelAbilityFromSource(object source);
    }

    public interface IAbilitySystemEvent
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }

        public event AbilitySpecEventHandler OnActivatedAbility;
        public event AbilitySpecEventHandler OnReleasedAbility;
        public event AbilitySpecEventHandler OnEndedAbility;

        public event AbilitySpecHandler OnGrantedAbility;
        public event AbilitySpecHandler OnRemovedAbility;
    }

    public static class AbilitySystemUtility
    {
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

    }

    [DefaultExecutionOrder(AbilitySystemExecutionOrder.MAIN_ORDER)]
    [AddComponentMenu("StudioScor/AbilitySystem/AbilitySystem Component", order: 0)]
    public class AbilitySystemComponent : BaseMonoBehaviour, IAbilitySystem, IAbilitySystemEvent
    {
        [Header(" [ Setup] ")]
        [SerializeField] private FAbility[] _InitAbilities;

        private readonly Dictionary<Ability, IAbilitySpec> _Abilities = new();
        private readonly List<IUpdateableAbilitySpec> updateableAbilitySpecs = new();

        public IReadOnlyDictionary<Ability,IAbilitySpec> Abilities => _Abilities;

        public event AbilitySpecEventHandler OnActivatedAbility;
        public event AbilitySpecEventHandler OnReleasedAbility;
        public event AbilitySpecEventHandler OnEndedAbility;

        public event AbilitySpecHandler OnGrantedAbility;
        public event AbilitySpecHandler OnRemovedAbility;

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
            Log("Setup Ability System");

            _Abilities.Clear();
        }

        public void ResetAbilitySystem()
        {
            Log("Reset Ability System");

            RemoveAllAbility();

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

            foreach (var ability in updateableAbilitySpecs)
            {
                ability.UpdateAbility(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            foreach (var ability in updateableAbilitySpecs)
            {
                ability.FixedUpdateAbility(deltaTime);
            }
        }

        public bool CanActivateAbility(Ability ability)
        {
            if(Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                return spec.CanActiveAbility();
            }
            else
            {
                return false;
            }
        }
        public bool IsActivateAbility(Ability ability)
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

        public bool TryActivateAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                return spec.TryActiveAbility();
            }
            else
            {
                return false;
            }
        }

        public void ForceActivateAbility(Ability ability)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                spec.ForceActiveAbility();
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

            return ability.CanGrantAbility(this);
        }
        public void ForceGrantAbility(Ability ability, int level = 0)
        {
            if (Abilities.TryGetValue(ability, out IAbilitySpec spec))
            {
                spec.OnOverride(level);

                return;
            }

            var abilitySpec = ability.CreateSpec(this, level);

            if (abilitySpec is IAbilitySpecEvent abilitySpecEvent)
            {
                abilitySpecEvent.OnActivatedAbility += Callback_OnActivatedAbility;
                abilitySpecEvent.OnReleasedAbility += Callback_OnReleasedAbility;
                abilitySpecEvent.OnEndedAbility += Callback_OnEndedAbility;
            }

            abilitySpec.GrantAbility();

            _Abilities.Add(ability, abilitySpec);

            if(abilitySpec is IUpdateableAbilitySpec updateableAbility)
            {
                updateableAbilitySpecs.Add(updateableAbility);
            }

            Callback_OnGrantedAbility(abilitySpec);
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

                if (spec is IAbilitySpecEvent abilitySpecEvent)
                {
                    abilitySpecEvent.OnActivatedAbility -= Callback_OnActivatedAbility;
                    abilitySpecEvent.OnReleasedAbility -= Callback_OnReleasedAbility;
                    abilitySpecEvent.OnEndedAbility -= Callback_OnEndedAbility;
                }

                spec.RemoveAbility();

                Callback_OnRemovedAbility(spec);

                _Abilities.Remove(ability);

                if(spec is IUpdateableAbilitySpec updateableAbility)
                {
                    updateableAbilitySpecs.Remove(updateableAbility);
                }
            }
        }
        public void RemoveAllAbility()
        {
            foreach (var ability in _Abilities.Keys)
            {
                ForceRemoveAbility(ability);
            }

            _Abilities.Clear();
        }


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

        protected virtual void Callback_OnActivatedAbility(IAbilitySpecEvent abilitySpec)
        {
            Log("On Activated Ability - " + abilitySpec.Ability.Name);

            OnActivatedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void Callback_OnReleasedAbility(IAbilitySpecEvent abilitySpec)
        {
            Log("On Released Ability - " + abilitySpec.Ability.Name);

            OnReleasedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void Callback_OnEndedAbility(IAbilitySpecEvent abilitySpec)
        {
            Log("On Ended Ability - " + abilitySpec.Ability.Name);

            OnEndedAbility?.Invoke(this, abilitySpec);
        }

        #endregion
    }
}
