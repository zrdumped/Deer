using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace MalbersAnimations.Utilities
{
    [CustomEditor(typeof(ActiveMeshes))]
    public class ActiveMeshesEditor : Editor
    {
        private ReorderableList list;
        SerializedProperty ToggleMeshes;
        private ActiveMeshes MyMeshes;

        private void OnEnable()
        {
            MyMeshes = (ActiveMeshes)target;

            ToggleMeshes = serializedObject.FindProperty("Meshes");

            list = new ReorderableList(serializedObject, ToggleMeshes, true, true, true, true);

            list.drawElementCallback = DrawElementCallback;
            list.drawHeaderCallback = HeaderCallbackDelegate;
            list.onAddCallback = OnAddCallBack;
        }

        /// <summary>
        /// Reordable List Header
        /// </summary>
        void HeaderCallbackDelegate(Rect rect)
        {
            Rect R_0 = new Rect(rect.x , rect.y,15, EditorGUIUtility.singleLineHeight);
            Rect R_1 = new Rect(rect.x + 14, rect.y, (rect.width - 10) / 2, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(R_1, "Meshes List ", EditorStyles.miniLabel);

            //Rect R_2 = new Rect(rect.x + 14 + ((rect.width - 30) / 2), rect.y, rect.width - ((rect.width - 10) / 2), EditorGUIUtility.singleLineHeight);

            MyMeshes.showMeshesList  = EditorGUI.ToggleLeft(R_0,"", MyMeshes.showMeshesList);
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = MyMeshes.Meshes[index];
            rect.y += 2;

            Rect R_1 = new Rect(rect.x, rect.y, (rect.width-50) / 2 + 25, EditorGUIUtility.singleLineHeight);
            Rect R_2 = new Rect(rect.x + 25 + ((rect.width - 30) / 2), rect.y, rect.width - ((rect.width) / 2)-14, EditorGUIUtility.singleLineHeight);


            element.Name = EditorGUI.TextField(R_1, element.Name,EditorStyles.label);
            if (GUI.Button(R_2, "Change",EditorStyles.miniButton))
            {
                ToggleButton(index);
            }
        }

        void ToggleButton(int index)
        {
            if (MyMeshes.Meshes[index].meshes != null)
            {
                MyMeshes.Meshes[index].ChangeMesh();
            }
        }

        void OnAddCallBack(ReorderableList list)
        {
            if (MyMeshes.Meshes == null)
            {
                MyMeshes.Meshes = new System.Collections.Generic.List<ActiveSMesh>();
            }

            MyMeshes.Meshes.Add(new ActiveSMesh()); 
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Toggle || Swap Meshes", MessageType.None);
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                list.DoLayoutList();
                EditorGUI.indentLevel++;
                if (MyMeshes.showMeshesList)
                {
                    if (list.index != -1)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        SerializedProperty Element = ToggleMeshes.GetArrayElementAtIndex(list.index);

                        EditorGUILayout.PropertyField(Element, true);
                        EditorGUILayout.EndVertical();
                    }
                }
                EditorGUI.indentLevel--;
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
