using UnityEditor;
using UnityEngine;
using StudioScor.Utilities.Editor;

namespace StudioScor.AbilitySystem.Editor
{
    [CustomEditor(typeof(AbilitySystemComponent))]
    [CanEditMultipleObjects]
    public sealed class AbilitySystemComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (Application.isPlaying)
            {
                GUILayout.Space(5f);
                SEditorUtility.GUI.DrawLine(4f);
                GUILayout.Space(5f);

                var abilitySystem = (AbilitySystemComponent)target;

                var abilitise = abilitySystem.Abilities;

                GUIStyle title = new();
                GUIStyle playing = new();
                GUIStyle wait = new();

                title.normal.textColor = Color.white;
                title.alignment = TextAnchor.MiddleCenter;
                title.fontStyle = FontStyle.Bold;

                playing.normal.textColor = Color.green;
                wait.normal.textColor = Color.gray;

                GUILayout.Label("[ Abilities ]", title);

                if (abilitise is not null)
                {
                    foreach (var ability in abilitise)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(ability.Key.ID);
                        GUILayout.Label("[ Level : " + ability.Value.Level.ToString() + " ]");
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(ability.Value.IsPlaying ? "[ Playing ]" : "[ Wait ]", ability.Value.IsPlaying ? playing : wait);
                        GUILayout.Space(10f);
                        GUILayout.EndHorizontal();

                        SEditorUtility.GUI.DrawLine(1f);
                    }
                }
            }
        }
    }
}
