using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MalbersAnimations.Utilities
{
    [Serializable]
    public class BoneRotation
    {
        public Transform bone;                                      //The bone
        public Vector3 offset = new Vector3(0, -90, -90);           //The offset for the look At
        [Range(0, 1)]
        internal float weight = 1;                                  //the Weight of the look a
        internal Quaternion InitRot;
    }


    public class LookAt : MonoBehaviour, IAnimatorListener       //This is for sending messages from the animator
    {
        public bool active = true;                                  //For Activating and Deactivating the HeadTrack

        public bool UseCamera;                          //Use the camera ForwardDirection instead a Target
        public Transform Target;                        //Use a target to look At

        [Space]
        public float LimitAngle = 80f;                  //Max Angle to LookAt
        public float Smoothness = 5f;                   //Smoothness between Enabled and Disable

        [Space]
        public BoneRotation[] Bones;                    //Bone Chain   

        private Transform cam;                          //Reference for the camera
        protected float angle;                          //Angle created between the transform.Forward and the LookAt Point    
        protected Vector3 direction;
        float moreRestore = 1;
        public bool debug = true;
        bool hastarget;


        void Start()
        {
            if (Camera.main != null) cam = Camera.main.transform;   //Get the main camera
        }

        void LateUpdate()
        {

            LookAtBoneSet();        //Rotate the bones
        }

        /// <summary>
        /// Enable or Disable this script functionality
        /// </summary>
        public void EnableLookAt(bool value)
        {
            active = value;
        }

        /// <summary>
        /// Rotates the bones to the Look direction (Unfortunately with no Lerp between them
        /// </summary>
        void LookAtBoneSet()
        {
            hastarget = true;
            if (!UseCamera && !Target) hastarget = false;


            foreach (var bone in Bones)
            {
                if (!bone.bone) continue;

                Vector3 dir = transform.forward;

                if (Target) dir = (Target.position - bone.bone.position).normalized;
                if (UseCamera) dir = cam.forward;

                direction = Vector3.Lerp(direction, dir, Time.deltaTime * Smoothness);



                angle = Vector3.Angle(transform.forward, direction); //Set the angle for the current bone

                if ((angle < LimitAngle && active && hastarget))
                {
                    var final = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(bone.offset);
                    var next = Quaternion.Lerp(bone.InitRot, final, bone.weight);
                    bone.InitRot = Quaternion.Lerp(bone.InitRot, next, Time.deltaTime * Smoothness * 2);
                    moreRestore = 1;
                    if (debug) Debug.DrawRay(bone.bone.position, direction * 5, Color.green);
                }
                else
                {
                    moreRestore += Time.deltaTime;
                    bone.InitRot = Quaternion.Lerp(bone.InitRot, bone.bone.rotation, Time.deltaTime * Smoothness * 2 * moreRestore);

                    moreRestore = Mathf.Clamp(moreRestore, 0, 1000); //this is for not making bigger than 1000
                }
                bone.bone.rotation = bone.InitRot;
            }
        }

        public virtual void NoTarget()
        {
            Target = null;
        }

        /// <summary>
        /// This is used to listen the Animator asociated to this gameObject
        /// </summary>
        public virtual void OnAnimatorBehaviourMessage(string message, object value)
        {
            this.InvokeWithParams(message, value);
        }
    }
}


