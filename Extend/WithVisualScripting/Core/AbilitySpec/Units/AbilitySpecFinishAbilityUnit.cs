#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Finish Ability")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecFinishAbilityUnit : AbilitySpecFlowUnit
    {
        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySpec = flow.GetValue<IAbilitySpec>(Target);

            abilitySpec.ForceFinishAbility();

            return Exit;
        }
    }
}

#endif