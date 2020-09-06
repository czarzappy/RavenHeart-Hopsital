using UnityEditor;

namespace Noho.Unity.Editor.Editors
{
    // [CustomEditor(typeof(GunkInitData))]
    // [CanEditMultipleObjects]
    public class GunkInitDataEditor : UnityEditor.Editor
    {
        SerializedProperty gunk;
        SerializedProperty amount;
    
        void OnEnable()
        {
            gunk = serializedObject.FindProperty("GunkTag");
            amount = serializedObject.FindProperty("Amount");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField($"[CUSTOM:{this}]");
            
            serializedObject.Update();
            // gunk.stringValue = EditorGUILayout.TagField(gunk.stringValue);
            EditorGUILayout.PropertyField(gunk);
            EditorGUILayout.PropertyField(amount);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
