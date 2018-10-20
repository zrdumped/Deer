using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace MalbersAnimations
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Animal))]
    public class AnimalEditor : Editor
    {
        protected Animal myAnimal;
        MonoScript script;
        protected SerializedProperty
            animalTypeID, GroundLayer, StartSpeed, height, WalkSpeed,WalkASpeed, TrotSpeed, TrotASpeed, RunSpeed, RunASpeed, TurnSpeed, FallRayDistance,
            maxAngleSlope, GotoSleep, SnapToGround, swapSpeed, waterLine, swimSpeed, SwimAnimSpeed, swimTurn,
            life, defense, attackStrength, FallRayMultiplier, debug;
        protected bool
            ground = true,
            water = true,
            advanced = true,
            atributes = true,
        Aevents = true;

        private void OnEnable()
        {
            myAnimal = (Animal)target;
            script = MonoScript.FromMonoBehaviour(myAnimal);
            FindProperties();
        }


        protected void FindProperties()
        {
            animalTypeID = serializedObject.FindProperty("animalTypeID");
            GroundLayer =  serializedObject.FindProperty("GroundLayer");
            StartSpeed =   serializedObject.FindProperty("StartSpeed");
            height =       serializedObject.FindProperty("height");

            WalkSpeed = serializedObject.FindProperty("WalkSpeed");
            TrotSpeed = serializedObject.FindProperty("TrotSpeed");
            RunSpeed = serializedObject.FindProperty("RunSpeed");

            WalkASpeed = serializedObject.FindProperty("WalkAnimSpeed");
            TrotASpeed = serializedObject.FindProperty("TrotAnimSpeed");
            RunASpeed = serializedObject.FindProperty("RunAnimSpeed");

            TurnSpeed = serializedObject.FindProperty("TurnSpeed");
            maxAngleSlope = serializedObject.FindProperty("maxAngleSlope");
            GotoSleep = serializedObject.FindProperty("GotoSleep");
            SnapToGround = serializedObject.FindProperty("SnapToGround");
            swapSpeed = serializedObject.FindProperty("swapSpeed");
            waterLine = serializedObject.FindProperty("waterLine");

            swimSpeed = serializedObject.FindProperty("swimSpeed");
            SwimAnimSpeed = serializedObject.FindProperty("SwimAnimSpeed");
            swimTurn = serializedObject.FindProperty("swimTurn");

            life = serializedObject.FindProperty("life");
            defense = serializedObject.FindProperty("defense");
            attackStrength = serializedObject.FindProperty("attackStrength");

            FallRayDistance = serializedObject.FindProperty("FallRayDistance");
            FallRayMultiplier = serializedObject.FindProperty("FallRayMultiplier");

            debug = serializedObject.FindProperty("debug");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawAnimalInspector();

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawAnimalInspector()
        {
            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Locomotion System", MessageType.None,true);
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            EditorGUI.BeginDisabledGroup(true);
            script = (MonoScript)EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            myAnimal.animalTypeID = EditorGUILayout.IntField(new GUIContent("Animal Type ID", "Enable the Additive Pose Animation to offset the Bones"), myAnimal.animalTypeID);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            ground = EditorGUILayout.Foldout(ground, "Ground");
            EditorGUI.indentLevel--;
            if (ground)
            {
                EditorGUILayout.PropertyField(GroundLayer, new GUIContent("Ground Layer", "Specify wich layer are Ground"));
                EditorGUILayout.PropertyField(StartSpeed, new GUIContent("Start Speed", "Activate the correct additive Animation to offset the Bones"));
                EditorGUILayout.PropertyField(height, new GUIContent("Height", "Distance from the ground"));

                EditorGUILayout.Separator();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(WalkSpeed, new GUIContent("Walk Speed", "Additional Walk Speed ... Greater than 0 the animal will slide"), GUILayout.MinWidth(20));
                EditorGUIUtility.labelWidth = 15;
                EditorGUILayout.PropertyField(WalkASpeed, new GUIContent("A", "Animator Walk Speed"), GUILayout.MaxWidth(50));
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(TrotSpeed, new GUIContent("Trot Speed", "Additional Trot Speed ... Greater than 0 the animal will slide"), GUILayout.MinWidth(20));
                EditorGUIUtility.labelWidth = 15;
                EditorGUILayout.PropertyField(TrotASpeed, new GUIContent("A", "Animator Trot Speed"),  GUILayout.MaxWidth(50));
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(RunSpeed, new GUIContent("Run Speed", "Additional Run Speed... Greater than 0 the animal will slide"), GUILayout.MinWidth(20));
                EditorGUIUtility.labelWidth = 15;
                EditorGUILayout.PropertyField(RunASpeed, new GUIContent("A", "Animator Run Speed"),GUILayout.MaxWidth(50));
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(TurnSpeed, new GUIContent("Turn Speed", "Add Speed to the turn"));
            }
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;


            advanced = EditorGUILayout.Foldout(advanced, "Advanced");
            EditorGUI.indentLevel--;
            if (ground)
            { 
                EditorGUILayout.PropertyField(maxAngleSlope, new GUIContent("Max Angle Slope", "Max Angle that the animal can walk"));
                myAnimal.SlowSlopes = EditorGUILayout.Toggle(new GUIContent("Slow Slopes","if the animal is going uphill: Slow it down"), myAnimal.SlowSlopes);
                EditorGUILayout.PropertyField(GotoSleep, new GUIContent("Go to Sleep", "Number of Idles before going to sleep (AFK)"));
                EditorGUILayout.PropertyField(SnapToGround, new GUIContent("Snap to ground", "Smoothness to aling to terrain"));
                EditorGUILayout.PropertyField(swapSpeed, new GUIContent("Swap Speed", "Swap the Speed with Shift instead of 1 2 3"));
                EditorGUILayout.PropertyField(FallRayDistance, new GUIContent("Front Fall Ray", "Multiplier to set the Fall Ray in front of the animal"));
                EditorGUILayout.PropertyField(FallRayMultiplier, new GUIContent("Fall Ray Multiplier", "Multiplier for the Fall Ray"));


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Locomotion Speed", "This are the values for the Animator Locomotion Blend Tree when the velocity is changed"), GUILayout.MaxWidth(120));
                myAnimal.movementS1 = EditorGUILayout.FloatField(myAnimal.movementS1, GUILayout.MinWidth(28));
                myAnimal.movementS2 = EditorGUILayout.FloatField(myAnimal.movementS2, GUILayout.MinWidth(28));
                myAnimal.movementS3 = EditorGUILayout.FloatField(myAnimal.movementS3, GUILayout.MinWidth(28));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();


            //────────────────────────────────── Water ──────────────────────────────────

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            water = EditorGUILayout.Foldout(water, "Water");
            EditorGUI.indentLevel--;
            if (water)
            {
             
                EditorGUILayout.PropertyField(waterLine, new GUIContent("Water Line", "Aling the animal to the Water Surface"));


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(swimSpeed, new GUIContent("Swim Speed", "Additional Swim Speed... moves the character"), GUILayout.MinWidth(20));
                EditorGUIUtility.labelWidth = 15;
                EditorGUILayout.PropertyField(SwimAnimSpeed, new GUIContent("A", "Animator Swim Speed"), GUILayout.MaxWidth(50));
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();
                

                EditorGUILayout.PropertyField(swimTurn, new GUIContent("Swim Turn", "Add Speed to the run... Greater than 0 the animal will Slide"));
             

            }
            EditorGUILayout.EndVertical();

            //────────────────────────────────── Atributes ──────────────────────────────────

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            atributes = EditorGUILayout.Foldout(atributes, "Atributes");
            EditorGUI.indentLevel--;
            if (atributes)
            {
                EditorGUILayout.PropertyField(life, new GUIContent("Life", "Life Points"));
                EditorGUILayout.PropertyField(defense, new GUIContent("Defense", "Defense Points"));
                EditorGUILayout.PropertyField(attackStrength, new GUIContent("Attack", "Attack Points"));

                myAnimal.attackDelay = EditorGUILayout.FloatField(new GUIContent("Attack Delay", "Time for this animal to be able to Attack again. \nGreater number than the animation itself will be ignored"), myAnimal.attackDelay);
                myAnimal.damageDelay = EditorGUILayout.FloatField(new GUIContent("Damage Delay", "Time for this animal can receive damage again"), myAnimal.damageDelay);
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            myAnimal.ShowEventsEditor = EditorGUILayout.Foldout(myAnimal.ShowEventsEditor, "Events");
            EditorGUI.indentLevel--;
            if (myAnimal.ShowEventsEditor)
            {

            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnAttack"), new GUIContent("On Attack"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnGetDamaged"), new GUIContent("On Get Damaged"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnDeathE"), new GUIContent("On Death"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("OnAction"), new GUIContent("On Action"));
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(debug, new GUIContent("Debug"));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Animal Values Changed");
                EditorUtility.SetDirty(target);
            }
        }
    }
}