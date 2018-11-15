using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations
{
    public class AIDeer : MonoBehaviour
    {
        private iMalbersInputs character;
        public bool aiControl = false;
        public Transform target = null;
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }
        private Transform Liam;
        bool arrive = false;
        int turnBackFrame = 0;

        void Awake()
        {
            //get the animalScript
            character = GetComponent<iMalbersInputs>();
            Liam = GameObject.Find("Liam").transform;
        }

        // Use this for initialization
        void Start()
        {
            if (aiControl)
            {
                agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();

                agent.updateRotation = false;
                agent.updatePosition = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (aiControl)
            {
                if (target != null)
                {
                    agent.SetDestination(target.position);
                    //Debug.Log("source");
                }

                //Debug.Log(agent.remainingDistance + " " + agent.stoppingDistance);
                if (agent.remainingDistance > agent.stoppingDistance )
                {
                    //Debug.Log("Moving");
                    character.Move(agent.desiredVelocity);
                }
                else
                {
                        character.Move(Vector3.zero);
                        GetComponent<MalbersAnimations.Utilities.LookAt>().active = true;
                }
            }
        }
    }
}
