using UnityEngine;
using System.Collections;

namespace MalbersAnimations
{
    /// <summary>
    /// Counts the cylces to change to the next animation
    /// </summary>
    public class SleepBehavior : StateMachineBehaviour
    {
        public bool CyclesFromController;

        public int Cycles, transitionID;

        int currentCycle;
        Animal animal;

        void CyclesToSleep(Animator animator, AnimatorStateInfo stateInfo)
        {
            if (CyclesFromController)
            {
                Cycles = animal.GotoSleep;
                if (Cycles == 0) return;
            }
            currentCycle++;

            if (currentCycle >= Cycles)
            {
                animal.SetIntID(transitionID);
                currentCycle = 0;
            }
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animal = animator.GetComponent<Animal>();

            if (!animal) return;

            if (animal.GotoSleep == 0) return;
         
            if (!stateInfo.IsTag("Idle"))
            {
                CyclesToSleep(animator, stateInfo);
            }
        }


        override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            animal = animator.GetComponent<Animal>();
            if (!animal) return;

            if (animal.GotoSleep == 0) return;

            if (animal.Tired == 0)
                animal.SetIntID(0);

            //If is in idle, start to count , to get to sleep
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
            {
                animal.Tired++;
                if (animal.Tired >= animal.GotoSleep - 1)
                {
                    //Get to the Sleep Mode
                    animal.SetIntID(-100);
                    animal.Tired = 0;
                }
            }
            else
            {
                CyclesToSleep(animator, animator.GetCurrentAnimatorStateInfo(0));
            }
        }
    }
}

