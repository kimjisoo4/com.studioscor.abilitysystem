#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("On Changed Ability Level")]
    [UnitSubtitle("AbilitySpec Event")]
    [UnitCategory("Events\\StudioScor\\AbilitySystem\\AbilitySpec")]
    public class AbilitySpecChangedAbilityLevelEventUnit : AbilitySpecLevelEventUnit
    {
        [DoNotSerialize]
        [PortLabel("PrevLevel")]
        public ValueOutput PrevLevel { get; private set; }
        protected override string hookName => AbilitySystemWithVisualScriptingEvent.ABILITYSPEC_CHANGED_ABILITY_LEVEL;

        protected override void Definition()
        {
            base.Definition();

            PrevLevel = ValueOutput<int>(nameof(PrevLevel));
        }
        protected override void AssignArguments(Flow flow, int args)
        {
            flow.SetValue(PrevLevel, args);
        }
    }
}

#endif