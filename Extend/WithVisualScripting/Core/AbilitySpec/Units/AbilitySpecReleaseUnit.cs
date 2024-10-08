#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting
{
    [UnitTitle("Release Spec")]
    [UnitSubtitle("AbilitySpec Unit")]
    public class AbilitySpecReleaseUnit : AbilitySpecFlowUnit
    {
        protected override ControlOutput EnterUnit(Flow flow)
        {
            var abilitySpec = flow.GetValue<IAbilitySpec>(Target);

            abilitySpec.ReleaseAbility();

            return Exit;
        }
    }
}

#endif