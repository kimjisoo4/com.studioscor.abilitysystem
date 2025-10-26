namespace StudioScor.AbilitySystem
{
    [System.Serializable]
    public struct FAbility
    {
        public FAbility(Ability ability, int level)
        {
            Ability = ability;
            Level = level;
            AbilityName = Ability.ID;
        }

        public string AbilityName;

        public Ability Ability;
        public int Level;
    }
}
