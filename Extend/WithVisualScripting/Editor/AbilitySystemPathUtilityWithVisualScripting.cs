#if SCOR_ENABLE_VISUALSCRIPTING
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Unity.VisualScripting;

using StudioScor.AbilitySystem.Editor;

namespace StudioScor.AbilitySystem.VisualScripting.Editor
{

    public static class AbilitySystemPathUtilityWithVisualScripting
    {
        public static string VisualScriptingResources => AbilitySystemPathUtility.RootFolder + "Extend/WithVisualScripting/Editor/Icons/";

        private readonly static Dictionary<string, EditorTexture> _EditorTextures = new Dictionary<string, EditorTexture>();

        public static EditorTexture Load(string name)
        {
            if (_EditorTextures.ContainsKey(name))
            {
                return GetStateTexture(name);
            }

            var _path = VisualScriptingResources;

            var editorTexture = EditorTexture.Single(AssetDatabase.LoadAssetAtPath<Texture2D>(_path + name + ".png"));

            _EditorTextures.Add(name, editorTexture);

            return GetStateTexture(name);
        }

        private static EditorTexture GetStateTexture(string name)
        {
            if(_EditorTextures.TryGetValue(name, out EditorTexture texture))
            {
                return texture;
            }
            else
            {
                return null;
            }
            
        }
    }
}
#endif