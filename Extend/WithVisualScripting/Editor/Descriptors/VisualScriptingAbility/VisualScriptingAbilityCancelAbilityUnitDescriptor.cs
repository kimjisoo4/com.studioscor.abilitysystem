﻿
#if SCOR_ENABLE_VISUALSCRIPTING
using Unity.VisualScripting;


namespace StudioScor.AbilitySystem.VisualScripting.Editor
{
    [Descriptor(typeof(AbilitySpecCancelAbilityUnit))]
    public sealed class VisualScriptingAbilityCancelAbilityUnitDescriptor : UnitDescriptor<AbilitySpecCancelAbilityUnit>
    {
        public VisualScriptingAbilityCancelAbilityUnitDescriptor(AbilitySpecCancelAbilityUnit target) : base(target)
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