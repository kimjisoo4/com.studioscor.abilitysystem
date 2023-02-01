
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{
    [Descriptor(typeof(AbilitySystemGrantAbilityEventUnit))]
    public sealed class AbilitySystemGrantAbilityEventDescriptor : UnitDescriptor<AbilitySystemGrantAbilityEventUnit>
    {
        public AbilitySystemGrantAbilityEventDescriptor(AbilitySystemGrantAbilityEventUnit target) : base(target)
        {
        }

        protected override EditorTexture DefaultIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySystem_Event_Grant_D");
        }
        protected override EditorTexture DefinedIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySystem_Event_Grant_D");
        }
    }
}
#endif