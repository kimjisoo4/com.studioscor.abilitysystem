
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;

namespace StudioScor.AbilitySystem.VisualScripting.Editor
{
    [Descriptor(typeof(AbilitySystemUnit))]
    public sealed class AbilitySystemUnitDescriptor : UnitDescriptor<AbilitySystemUnit>
    {
        public AbilitySystemUnitDescriptor(AbilitySystemUnit target) : base(target)
        {
        }

        protected override EditorTexture DefaultIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_VisualScriptingAbility_D");
        }
        protected override EditorTexture DefinedIcon()
        {
            return AbilitySystemPathUtilityWithVisualScripting.Load("T_Icon_VisualScriptingAbility_D");
        }
    }
}
#endif