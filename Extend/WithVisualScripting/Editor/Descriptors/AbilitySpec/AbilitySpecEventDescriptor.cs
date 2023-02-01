
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{

    [Descriptor(typeof(AbilitySpecEventUnit))]
    public sealed class AbilitySpecEventDescriptor : UnitDescriptor<AbilitySpecEventUnit>
    {
        public AbilitySpecEventDescriptor(AbilitySpecEventUnit target) : base(target)
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