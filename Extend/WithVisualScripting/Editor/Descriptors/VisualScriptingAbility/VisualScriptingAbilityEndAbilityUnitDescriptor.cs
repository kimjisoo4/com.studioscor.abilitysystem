
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{

    [Descriptor(typeof(AbilitySpecFinishAbilityUnit))]
    public sealed class VisualScriptingAbilityEndAbilityUnitDescriptor : UnitDescriptor<AbilitySpecFinishAbilityUnit>
    {
        public VisualScriptingAbilityEndAbilityUnitDescriptor(AbilitySpecFinishAbilityUnit target) : base(target)
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