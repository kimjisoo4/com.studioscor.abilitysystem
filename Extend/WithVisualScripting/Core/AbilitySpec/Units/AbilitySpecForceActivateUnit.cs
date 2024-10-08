#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Force Activate Spec")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecForceActivateUnit : AbilitySpecFlowUnit
    {
        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySpec = flow.GetValue<IAbilitySpec>(Target);

            abilitySpec.ForceActiveAbility();

            return Exit;
        }
    }
}

#endif