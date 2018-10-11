using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    /// <summary>
    /// This Behavior Updates and resets all parameters to their original state
    /// </summary>
    public class RecoverBehavior : StateMachineBehaviour
    {
        public float Smoothness = 0;

        Animal animal;
        Rigidbody rb;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();                           //Get Reference for Animal
            rb = animator.GetComponent<Rigidbody>();                            //Get Reference for Rigid Body

            animal.IsInAir = false;
            rb.constraints = animal.Still_Constraints;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            animal.IsInAir = false;
          
            if (stateInfo.normalizedTime < 0.9f)   //Smooth Stop when RecoverFalls
            {
                rb.drag = Mathf.Lerp(rb.drag, 3, Time.deltaTime * (10f+Smoothness));
            }
            else
            {
                animator.applyRootMotion = true;
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.applyRootMotion != true) animator.applyRootMotion = true;
           
            rb.drag = 0; //Reset the Drag
        }
    }
}