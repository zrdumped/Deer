using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Torch))]
public class NewBehaviourScript1 : Editor
{
    private SerializedObject torch;
    private SerializedProperty type, lineTo, speed;
    void OnEnable()
    {
        torch = new SerializedObject(target);
        type = torch.FindProperty("type");
        lineTo = torch.FindProperty("lineTo");
        //trail = torch.FindProperty("trail");
        speed = torch.FindProperty("speed");
        //trailParticleSystem = torch.FindProperty("trailParticleSystem");
    }
    public override void OnInspectorGUI()
    {
        torch.Update();
        EditorGUILayout.PropertyField(type);
        if (type.enumValueIndex == 0)
        {
            
        }
        else if (type.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(lineTo);
            //EditorGUILayout.PropertyField(trail);
            //EditorGUILayout.PropertyField(trailParticleSystem);
            EditorGUILayout.PropertyField(speed);
        }
        torch.ApplyModifiedProperties();
    }
}