
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{
    [Descriptor(typeof(AbilitySpecUnit))]
    public sealed class AbilitySpecUnitDescriptor : UnitDescriptor<AbilitySpecUnit>
    {
        public AbilitySpecUnitDescriptor(AbilitySpecUnit target) : base(target)
        {
        }
        protected override EditorTexture DefaultIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_VisualScriptingAbilitySpec_D");
        }
        protected override EditorTexture DefinedIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_VisualScriptingAbilitySpec_D");
        }
    }
}
#endif