using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Key))]
public class NewBehaviourScript2 : Editor
{
    private SerializedObject key;
    private SerializedProperty pattern;
    void OnEnable()
    {
        key = new SerializedObject(target);
        pattern = key.FindProperty("pattern");
    }
    public override void OnInspectorGUI()
    {
        key.Update();
        //for(int i = 0; i < patternNum; i++)
        EditorGUILayout.PropertyField(pattern, true);
        key.ApplyModifiedProperties();
    }
}