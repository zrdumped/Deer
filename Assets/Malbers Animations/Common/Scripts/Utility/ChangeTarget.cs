using UnityEngine;

namespace MalbersAnimations
{
  public class ChangeTarget : MonoBehaviour
    {
        public Transform[] targets;
        public KeyCode key = KeyCode.T;
        int current;
        public bool NoInputs = false;
        MFreeLookCamera m;

        // Update is called once per frame

        void Start()
        {
            if (NoInputs)
            {
                MalbersInput input = null;
                for (int i = 0; i < targets.Length; i++)
                {
                    input = targets[i].GetComponent<MalbersInput>();
                    if (input) input.enabled = false;
                }

                m = GetComponent<MFreeLookCamera>();
                if (m)
                {
                    input = m.Target.GetComponent<MalbersInput>();
                    if (input) input.enabled = true;

                    for (int i = 0; i < targets.Length; i++)
                    {
                        if (targets[i] == m.Target)
                        {
                            current = i;
                            break;
                        }
                    }
                }
            } 
        }

        void Update()
        {
            if (Input.GetKeyDown(key))
            {
                if (NoInputs)
                {
                    MalbersInput input = targets[current].GetComponent<MalbersInput>();
                    if (input) input.enabled = false;
                }

                current++;
                current = current % targets.Length;
                SendMessage("SetTarget", targets[current]);
                //m.SetTarget(targets[current]);

                if (NoInputs)
                {
                    MalbersInput input = targets[current].GetComponent<MalbersInput>();
                    if (input) input.enabled = true;
                }
            }
        }
    }
}
