
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{
    [Descriptor(typeof(VisualScriptingAbilityChangeAbilityLevelEventUnit))]
    public sealed class VisualScriptingAbilitySpecLevelChangedEventDescriptor : UnitDescriptor<VisualScriptingAbilityChangeAbilityLevelEventUnit>
    {
        public VisualScriptingAbilitySpecLevelChangedEventDescriptor(VisualScriptingAbilityChangeAbilityLevelEventUnit target) : base(target)
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