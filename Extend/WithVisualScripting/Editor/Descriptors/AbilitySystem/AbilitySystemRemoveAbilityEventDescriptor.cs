
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{
    [Descriptor(typeof(AbilitySystemRemoveAbilityEventUnit))]
    public sealed class AbilitySystemRemoveAbilityEventDescriptor : UnitDescriptor<AbilitySystemRemoveAbilityEventUnit>
    {
        public AbilitySystemRemoveAbilityEventDescriptor(AbilitySystemRemoveAbilityEventUnit target) : base(target)
        {
        }

        protected override EditorTexture DefaultIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySystem_Event_Remove_D");
        }
        protected override EditorTexture DefinedIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySystem_Event_Remove_D");
        }
    }
}
#endif