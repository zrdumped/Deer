using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MalbersAnimations
{
    public class JumpBehaviour : StateMachineBehaviour
    {
        [Header("Checking Fall")]
        public float fallRay = 1.7f;       //Ray to Check if the Terrain is the same
        public float treshold = 0.5f;      //for calculating something     
        public float willFall = 0.7f;

        [Header("Jump Up Cliff")]
        public float startEdge = 0.5f;
        public float finishEdge = 0.6f;
        public float CliffRay = 0.6f;

        float jumpPoint;
        RaycastHit JumpRay;
        Animal animal;


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();

            jumpPoint = animator.transform.position.y;
            animal.InAir(true);
            animal.SetIntID(0);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //This code is execute when the animal can change to fall state if there's no future ground to land on
            if (Physics.Raycast(animal.Pivot_fall, -animal.transform.up, out JumpRay, animal.Pivot_Chest.multiplier * animal.ScaleFactor * fallRay, animal.GroundLayer))
            {
                if (animal.debug)
                {
                    Debug.DrawRay(animal.Pivot_fall, -animal.transform.up * animal.Pivot_Chest.multiplier * animal.ScaleFactor * fallRay, Color.red);
                }


                if ((jumpPoint - JumpRay.point.y) <= treshold * animal.ScaleFactor)   //If if finding a lower jump point;
                {
                    animal.SetIntID(0); //Keep the INTID in 0
                }
                else
                {
                    if (stateInfo.normalizedTime > willFall) animal.SetIntID(111); //Set INTID to 111 to activate the FALL transition
                }
            }
            else
            {
                if (stateInfo.normalizedTime > willFall) animal.SetIntID(111); //Set INTID to 111 to activate the FALL transition
            }

            //-----------------------------------------Get jumping on a cliff -------------------------------------------------------------------------------

            if (stateInfo.normalizedTime >= startEdge && stateInfo.normalizedTime <= finishEdge)
            {
                Debug.DrawRay(animal.Pivot_Chest.GetPivot + (animal.transform.forward * 0.2f), -animal.transform.up * CliffRay * animal.ScaleFactor, Color.black);

                if (Physics.Raycast(animal.Pivot_Chest.GetPivot, -animal.transform.up, out JumpRay, CliffRay * animal.ScaleFactor, animal.GroundLayer))
                {
                    animal.SetIntID(110);
                }
            }
        }

        //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animal.RealAnimatorState("Fall"))
            {
                animal.IsInAir = false;
            }
            animal.SetIntID(0);
        }
    }
}