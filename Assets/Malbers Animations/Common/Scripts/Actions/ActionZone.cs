using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MalbersAnimations.Events;
using UnityEngine.Events;
using System;

namespace MalbersAnimations
{
    [RequireComponent(typeof(BoxCollider))]
    public class ActionZone : MonoBehaviour
    {
        static Keyframe[] K = { new Keyframe(0, 0), new Keyframe(1, 1) };

        public Actions actionsToUse;

        public bool automatic;                          //Set the Action Zone to Automatic
        public int ID;                                  //ID of the Action Zone (Value)
        public int index;                               //Index of the Action Zone (List index)
        public float AutomaticDisabled = 1f;            //is Automatic is set to true this will be the time to disable temporarly the Trigger
        public bool HeadOnly;                           //Use the Trigger for heads only


        public bool Align;                              //Align the Animal entering to the Aling Point
        public Transform AlingPoint;
        public float AlignTime = 0.5f;
        public AnimationCurve AlignCurve = new AnimationCurve(K);

        public bool AlignPos = true, AlignRot = true, AlignLookAt = false;

        protected List<Collider> _colliders;
        protected Animal animal;

        public AnimalEvent OnEnter = new AnimalEvent();
        public AnimalEvent OnExit = new AnimalEvent();
        public AnimalEvent OnAction = new AnimalEvent();


        public static List<ActionZone> ActionZones;

        //───────AI───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        public float stoppingDistance = 0.5f;
        public Transform NextTarget;


        void OnEnable()
        {
            if (ActionZones == null) ActionZones = new List<ActionZone>();

            ActionZones.Add(this);          //Save the the Action Zones
        }

        void OnDisable()
        {
            ActionZones.Remove(this);
        }


        void OnTriggerEnter(Collider other)
        {
            Animal animal = other.GetComponentInParent<Animal>();

            if (other.gameObject.layer != 20) return;                           //Just use the Colliders with the Animal Layer on it
                
            if (!animal) return;                                                //If there's no animal script found skip all

            if (_colliders == null)
                _colliders = new List<Collider>();                              //Check all the colliders that enters the Action Zone Trigger

            if (HeadOnly && !other.name.ToLower().Contains("head")) return;     //If is Head Only and no head was found Skip

            if (_colliders.Find(item => item == other) == null)                 //if the entering collider is not already on the list add it
            {
                _colliders.Add(other);
            }

            if (animal == this.animal) return;                      //if the animal is the same do nothing
            else
            {
                this.animal = animal;
            }

            animal.OnAction.AddListener(OnActionListener);          //Listen when the animal activate the Action Input

            OnEnter.Invoke(animal);
            animal.ActionEmotion(ID);

            if (automatic && animal.CurrentAnimState.IsTag("Locomotion"))       //Just activate when is on the Locomotion State if this is automatic
            {
                animal.EnableAction(true);
                StartCoroutine(ReEnable(animal));
                //this.animal.ActionID = -1;
                //this.animal = null;
                OnActionListener();
            }
        }

        void OnTriggerExit(Collider other)
        {
            Animal animal = other.GetComponentInParent<Animal>();
            if (!animal) return; //If there's no animal script found skip all

            if (animal != this.animal) return;

            if (HeadOnly && !other.name.Contains("Head")) return;

            if (_colliders.Find(item => item == other))     //Remove the collider that entered off the list.
            {
                _colliders.Remove(other);
            }

            if (_colliders.Count == 0)
            {
                OnExit.Invoke(animal);                              //Invoke On Exit when all colliders of the animal has exited the Trigger Zone
                animal.OnAction.RemoveListener(OnActionListener);   //Remove the Method fron the Action Listener
                animal.ActionEmotion(-1);                           //Reset the Action ID
                this.animal = null;
            }
        }

        /// <summary>
        /// This will disable the Collider on the action zone
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        IEnumerator ReEnable(Animal animal) //For Automatic only 
        {
            if (AutomaticDisabled > 0)
            {
                GetComponent<Collider>().enabled = false;
                yield return null;
                yield return null;
                animal.ActionEmotion(-1);
                yield return new WaitForSeconds(AutomaticDisabled);
                GetComponent<Collider>().enabled = true;
            }
            this.animal = null;     //Reset animal
            _colliders = null;      //Reset Colliders
            yield return null;
        }

        public virtual void _DestroyActionZone(float time)
        {
            Destroy(gameObject, time);
        }

        /// <summary>
        /// Used for checking if the animal press the action button
        /// </summary>
        private void OnActionListener()
        {
            if (!animal) return;

            OnAction.Invoke(animal);
            if (Align && AlingPoint)
            {
                IEnumerator ICo = null;

                if (AlignLookAt)
                {
                    ICo = Utilities.MalbersTools.AlignLookAtTransform(animal.transform, AlingPoint, AlignTime, AlignCurve);
                }
                else
                {
                    ICo = Utilities.MalbersTools.AlignTransformsC(animal.transform, AlingPoint, AlignTime, AlignPos, AlignRot, AlignCurve);
                }

                StartCoroutine(ICo);
            }

            StartCoroutine(CheckForCollidersOff());
            
            // animal.OnAction.RemoveListener(OnActionListener);

            //animal.ActionID = -1;
            //animal = null;
        }


        IEnumerator CheckForCollidersOff()
        {
            yield return null;
            yield return null;
            if (_colliders != null && _colliders[0] && _colliders[0].enabled == false)
            {
                animal.OnAction.RemoveListener(OnActionListener);
                animal.ActionID = -1;
                animal = null;
                _colliders = null;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (EditorAI)
            {
                //Debug.DrawLine(transform.position, NextTarget.transform.position, Color.green);
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, stoppingDistance);
            }

        }
#endif

        [HideInInspector] public bool EditorShowEvents = true;
        [HideInInspector] public bool EditorAI = true;
    }
}