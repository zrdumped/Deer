using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;


namespace MalbersAnimations
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AnimalAIControl))]
    public class AnimalAIControlEd : Editor
    {
        AnimalAIControl M;
        MonoScript script;

        private void OnEnable()
        {
            M = (AnimalAIControl)target;
            script = MonoScript.FromMonoBehaviour(M);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Basic AI system for Animal Script", MessageType.None, true);
            EditorGUILayout.EndVertical();


            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            EditorGUI.BeginDisabledGroup(true);
            script = (MonoScript)EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("target"), new GUIContent("Target","Target to follow"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

             EditorGUILayout.PropertyField(serializedObject.FindProperty("AutoSpeed"), new GUIContent("Auto Speed", "Target to follow"));
            if (serializedObject.FindProperty("AutoSpeed").boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ToTrot"), new GUIContent("To Trot", "Distance to start troting"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ToRun"), new GUIContent("To Run", "Distance to start Running"));
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debug"), new GUIContent("Debug"));
            EditorGUILayout.EndVertical();



            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Animal AI Control Changed");
                EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}