﻿#if SCOR_ENABLE_GAMEPLAYTAG
#endif
namespace StudioScor.AbilitySystem
{
    #region Events
    public delegate void AbilityEventHandler(IAbilitySpec abilitySpec);
    public delegate void AbilityLevelEventHandler(IAbilitySpec abilitySpec, int currentLevel, int prevLevel);
    #endregion

    public partial interface IAbilitySpec
    {
        public Ability Ability { get; }
        public AbilitySystem AbilitySystem { get; }

        public bool IsPlaying { get; }
        public int Level { get; }

        public void GrantAbility();
        public void RemoveAbility();


        public void OnOverride(int level);
        public void SetAbilityLevel(int level);


        public bool TryActiveAbility();
        public bool CanActiveAbility();
        public void ForceActiveAbility();

        public void ReleaseAbility();

        public void ForceCancelAbility();


        public void EndAbility();
        public void UpdateAbility(float deltaTime);
        public void FixedUpdateAbility(float deltaTime);

        public event AbilityEventHandler OnActivatedAbility;
        public event AbilityEventHandler OnEndedAbility;
        public event AbilityEventHandler OnFinishedAbility;
        public event AbilityEventHandler OnCanceledAbility;
        public event AbilityLevelEventHandler OnChangedAbilityLevel;
    }
}
