
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{
    [Descriptor(typeof(VisualScriptingAbilitySpecLevelEventUnit))]
    public sealed class VisualScriptingAbilitySpecLevelEventDescriptor : UnitDescriptor<VisualScriptingAbilitySpecLevelEventUnit>
    {
        public VisualScriptingAbilitySpecLevelEventDescriptor(VisualScriptingAbilitySpecLevelEventUnit target) : base(target)
        {
        }

        protected override EditorTexture DefaultIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySpec_Event_D");
        }
        protected override EditorTexture DefinedIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySpec_Event_D");
        }
    }
}
#endif