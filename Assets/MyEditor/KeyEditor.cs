using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Pattern))]
public class NewBehaviourScript2 : Editor
{
    private SerializedObject key;
    private SerializedProperty pattern;
    //private SerializedProperty minScale;
    //private SerializedProperty maxScale;
    //private SerializedProperty pitchLength;
    //private SerializedProperty intermissionLength, changeFrames;

    void OnEnable()
    {
        key = new SerializedObject(target);
        pattern = key.FindProperty("pattern");
    }
    public override void OnInspectorGUI()
    {
        key.Update();
        EditorGUILayout.PropertyField(key.FindProperty("minScale"));
        EditorGUILayout.PropertyField(key.FindProperty("maxScale"));
        EditorGUILayout.PropertyField(key.FindProperty("pitchLength"));
        EditorGUILayout.PropertyField(key.FindProperty("intermissionLength"));
        EditorGUILayout.PropertyField(key.FindProperty("changeFrames"));
        EditorGUILayout.PropertyField(key.FindProperty("onShow"));
        // EditorGUILayout.PropertyField(minScale);
        //EditorGUILayout.PropertyField(maxScale);
        //EditorGUILayout.PropertyField(pitchLength);
        //EditorGUILayout.PropertyField(intermissionLength);
        EditorGUILayout.PropertyField(pattern, true);
        key.ApplyModifiedProperties();
    }
}