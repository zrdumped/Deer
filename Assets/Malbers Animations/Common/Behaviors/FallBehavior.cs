using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    public class FallBehavior : StateMachineBehaviour
    {
        RaycastHit JumpRay;

        [Tooltip("The Lower Fall animation will set to 1 if this distance the current distance to the ground")]
        public float LowerDistance;
        float animalFloat;
        Animal animal;
        Rigidbody rb;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();
            rb = animator.GetComponent<Rigidbody>();


            animal.SetIntID(1);
            animal.IsInAir = true;
            animator.SetFloat("IDFloat", 1);


            animal.MaxHeight = 0; //Resets MaxHeight

            animator.applyRootMotion = false;
            rb.drag = 0;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            if (Physics.Raycast(animator.transform.position, -animal.transform.up, out JumpRay, 100, animal.GroundLayer))
            {
                if (animal.MaxHeight < JumpRay.distance)
                {
                    animal.MaxHeight = JumpRay.distance; //get the lower Distance 
                }
                //Blend between fall animations ... Higher 1 one animation
                animalFloat = Mathf.Lerp(animalFloat, 
                    Mathf.Lerp(1, 0, (animal.MaxHeight - JumpRay.distance) / (animal.MaxHeight - LowerDistance)),
                    Time.deltaTime * 20f);

                animator.SetFloat(HashIDsAnimal.IDFloatHash, animalFloat);
            }
        }
    }
}