namespace StudioScor.AbilitySystem
{
    #region Events
    public delegate void AbilityEventHandler(IAbilitySpecEvent abilitySpec);
    public delegate void AbilityLevelEventHandler(IAbilitySpecEvent abilitySpec, int currentLevel, int prevLevel);
    #endregion

    public enum EAbilitySpecEventType
    {
        Activated,
        Released,
        Ended,
        Finished,
        Canceled,
        OnChangedAbilityLevel,
    }

    public interface IAbilitySpecEvent
    {
        public Ability Ability { get; }

        public event AbilityEventHandler OnActivatedAbility;
        public event AbilityEventHandler OnReleasedAbility;
        public event AbilityEventHandler OnEndedAbility;
        public event AbilityEventHandler OnFinishedAbility;
        public event AbilityEventHandler OnCanceledAbility;
        public event AbilityLevelEventHandler OnChangedAbilityLevel;
    }

    public interface IAbilitySpec
    {
        public Ability Ability { get; }
        public IAbilitySystem AbilitySystem { get; }
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

        public void CancelAbilityFromSource(object source);
        public void CancelAbility();


        public void EndAbility();
        public void UpdateAbility(float deltaTime);
        public void FixedUpdateAbility(float deltaTime);
    }
}
