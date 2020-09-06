using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Components;

namespace ZEngine.Unity.Editor.Core.CustomEditors
{
    [CustomEditor(typeof(AttributeController))]
    public class LevelScriptEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            AttributeController myTarget = (AttributeController)target;

            if (myTarget.Attributes != null)
            {
                for (int i = 0; i < myTarget.Attributes.Count; i++)
                {
                    // DrawFoldoutInspector(myTarget.Attributes[i], ref this);
                    // myTarget.Attributes[i] = EditorGUILayout.ObjectField(myTarget.Attributes[i], myTarget.Attributes[i].GetType());
                }
            }
            DrawDefaultInspector();
            
            EditorGUILayout.HelpBox("Test", MessageType.Info);

            if (GUILayout.Button("Add FadeIn Attribute"))
            {
                if (myTarget.Attributes == null)
                {
                    myTarget.Attributes = new List<ZAttr>();
                }
                
                myTarget.Attributes.Add(new ColorFadeZAttr());
            }
            // EditorGUILayout.("Test")
            // myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
            // EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
        }
    }
}