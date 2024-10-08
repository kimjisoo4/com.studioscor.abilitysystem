#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Cancel Ability")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecCancelAbilityUnit : AbilitySpecFlowUnit
    {
        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySpec = flow.GetValue<IAbilitySpec>(Target);

            abilitySpec.CancelAbility();

            return Exit;
        }
    }
}

#endif