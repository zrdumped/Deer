using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MalbersAnimations
{
    [CustomEditor(typeof(TransformAnimation))]
    public class TransformAnimationEditor : Editor
    {
        TransformAnimation My;

        void OnEnable()
        {
            My = (TransformAnimation)target;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Use to animate a transform to this values", MessageType.None);
            EditorGUILayout.EndVertical();


            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                EditorGUIUtility.labelWidth = 50;
                My.time = EditorGUILayout.FloatField(new GUIContent("Time", "Duration of the animation"), My.time);
                My.delay = EditorGUILayout.FloatField(new GUIContent("Delay", "Delay before the animation is started"), My.delay);
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();

                //EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                //My.cycle =(AnimCycle) EditorGUILayout.EnumPopup(new GUIContent("Cycle", "Type of Cicle of the Transform Animation"), My.cycle);
                //EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                My.UsePosition = GUILayout.Toggle(My.UsePosition, new GUIContent("Position"), EditorStyles.miniButton);
                My.UseRotation = GUILayout.Toggle(My.UseRotation, new GUIContent("Rotation"), EditorStyles.miniButton);
                My.UseScale = GUILayout.Toggle(My.UseScale, new GUIContent("Scale"), EditorStyles.miniButton);
                EditorGUILayout.EndHorizontal();

                if (My.UsePosition)
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Position", GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
                    My.Position = EditorGUILayout.Vector3Field("", My.Position, GUILayout.MinWidth(120));
                    My.PosCurve = EditorGUILayout.CurveField(My.PosCurve, GUILayout.MinWidth(50));
                    EditorGUILayout.EndHorizontal();
                }

                if (My.UseRotation)
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Rotation", GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
                    My.Rotation = EditorGUILayout.Vector3Field("", My.Rotation, GUILayout.MinWidth(120));
                    My.RotCurve = EditorGUILayout.CurveField(My.RotCurve, GUILayout.MinWidth(50));
                    EditorGUILayout.EndHorizontal();
                }

                if (My.UseScale)
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Scale", GUILayout.MinWidth(50), GUILayout.MaxWidth(100));
                    My.Scale = EditorGUILayout.Vector3Field("", My.Scale, GUILayout.MinWidth(120));
                    My.ScaleCurve = EditorGUILayout.CurveField(My.ScaleCurve, GUILayout.MinWidth(50));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }

    }
}