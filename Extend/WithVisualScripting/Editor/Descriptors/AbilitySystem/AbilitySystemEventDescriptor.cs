
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting.Editor
{

    [Descriptor(typeof(AbilitySystemEventUnit))]
    public sealed class AbilitySystemEventDescriptor : UnitDescriptor<AbilitySystemEventUnit>
    {
        public AbilitySystemEventDescriptor(AbilitySystemEventUnit target) : base(target)
        {
        }

        protected override EditorTexture DefaultIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySystem_Event_D");
        }
        protected override EditorTexture DefinedIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_AbilitySystem_Event_D");
        }
    }
}
#endif