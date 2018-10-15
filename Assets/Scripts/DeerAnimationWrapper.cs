using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if CROSS_PLATFORM_INPUT
using UnityStandardAssets.CrossPlatformInput;
#endif

namespace MalbersAnimations {
    public class DeerAnimationWrapper: MonoBehaviour {

        private iMalbersInputs character;
        public float h = 0;  //Horizontal Right & Left   Axis X
        public float v = 0;  //Vertical   Forward & Back Axis Z


        // Use this for initialization
        void Start() {
            
        }

        void Awake() {
            //get the animalScript
            character = GetComponent<iMalbersInputs>();
        }

        // Update is called once per frame
        void Update() {
            character.Move(new Vector3(h, 0, v), false);
        }
    }
}


