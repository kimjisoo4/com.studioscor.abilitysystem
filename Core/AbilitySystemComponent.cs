using System.Collections.Generic;
using UnityEngine;
using System;
using StudioScor.Utilities;


namespace StudioScor.AbilitySystem
{
    public delegate void AbilitySpecHandler(IAbilitySystem abilitySystem, IAbilitySpec abilitySpec);

    public interface IAbilitySystem
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }

        public IReadOnlyDictionary<Ability, IAbilitySpec> Abilities { get; }

        public void Tick(float deltaTime);
        public void FixedTick(float deltaTime);

        public bool TryGrantAbility(Ability ability, int level = 0);
        public bool TryRemoveAbility(Ability ability);
        public bool TryActivateAbility(Ability ability);
        public void ReleasedAbility(Ability ability);
        public bool IsActivateAbility(Ability ability);
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

        private readonly Dictionary<Ability, IAbilitySpec> abilities = new();
        private readonly List<IUpdateableAbilitySpec> updateableAbilitySpecs = new();

        public IReadOnlyDictionary<Ability,IAbilitySpec> Abilities => abilities;

        public event AbilitySpecHandler OnActivatedAbility;
        public event AbilitySpecHandler OnReleasedAbility;
        public event AbilitySpecHandler OnEndedAbility;

        public event AbilitySpecHandler OnGrantedAbility;
        public event AbilitySpecHandler OnRemovedAbility;

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

        private void Awake()
        {
            Setup();
        }

        private void Start()
        {
            foreach (var ability in initAbilities)
            {
                TryGrantAbility(ability.Ability, ability.Level);
            }
        }
        protected void Setup()
        {
            Log("Setup Ability System");

            abilities.Clear();
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

            foreach (var ability in updateableAbilitySpecs)
            {
                ability.UpdateAbility(deltaTime);
            }
        }

        public override void FixedTick(float deltaTime)
        {
            base.FixedTick(deltaTime);

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

            abilitySpec.OnActivatedAbility += Callback_OnActivatedAbility;
            abilitySpec.OnReleasedAbility += Callback_OnReleasedAbility;
            abilitySpec.OnEndedAbility += Callback_OnEndedAbility;

            abilitySpec.GrantAbility();

            abilities.Add(ability, abilitySpec);

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

                if (spec is IAbilitySpec abilitySpecEvent)
                {
                    abilitySpecEvent.OnActivatedAbility -= Callback_OnActivatedAbility;
                    abilitySpecEvent.OnReleasedAbility -= Callback_OnReleasedAbility;
                    abilitySpecEvent.OnEndedAbility -= Callback_OnEndedAbility;
                }

                spec.RemoveAbility();

                Callback_OnRemovedAbility(spec);

                abilities.Remove(ability);

                if(spec is IUpdateableAbilitySpec updateableAbility)
                {
                    updateableAbilitySpecs.Remove(updateableAbility);
                }
            }
        }
        public void RemoveAllAbility()
        {
            foreach (var ability in abilities.Keys)
            {
                ForceRemoveAbility(ability);
            }

            abilities.Clear();
        }


        #region CallBack
        protected virtual void Callback_OnGrantedAbility(IAbilitySpec grantAbilitySpec)
        {
            Log("On Added Ability - " + grantAbilitySpec.Ability.AbilityName);

            OnGrantedAbility?.Invoke(this, grantAbilitySpec);
        }
        protected virtual void Callback_OnRemovedAbility(IAbilitySpec removeAbilitySpec)
        {
            Log("On Removed Ability - " + removeAbilitySpec.Ability.AbilityName);

            OnRemovedAbility?.Invoke(this, removeAbilitySpec);
        }

        protected virtual void Callback_OnActivatedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Activated Ability - " + abilitySpec.Ability.AbilityName);

            OnActivatedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void Callback_OnReleasedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Released Ability - " + abilitySpec.Ability.AbilityName);

            OnReleasedAbility?.Invoke(this, abilitySpec);
        }
        protected virtual void Callback_OnEndedAbility(IAbilitySpec abilitySpec)
        {
            Log("On Ended Ability - " + abilitySpec.Ability.AbilityName);

            OnEndedAbility?.Invoke(this, abilitySpec);
        }

        #endregion
    }
}
