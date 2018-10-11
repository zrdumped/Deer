using UnityEngine;
using System.Collections.Generic;
using System;
#if CROSS_PLATFORM_INPUT
using UnityStandardAssets.CrossPlatformInput;
#endif

namespace MalbersAnimations
{
    public enum InputType
    {
        Input, Key
    }

    public enum InputButton
    {
        Press, Down, Up
    }
    #region InputRow
    /// <summary>
    /// Input Class to change directly between Keys and Unity Inputs
    /// </summary>
    [Serializable]
    public class InputRow
    {
        public bool active = true;
        public string name = "Variable";
        public InputType type;
        public string input = "Value";
        public KeyCode key;
        public InputButton GetPressed;

        /// <summary>
        /// Return True or False to the Selected type of Input of choice
        /// </summary>
        public bool GetInput
        {
            get{
                if (!active) return false;
                switch (type)
                {
                    case InputType.Input:
                        switch (GetPressed)
                        {
                            case InputButton.Press:
                                #if !CROSS_PLATFORM_INPUT
                                return Input.GetButton(input);
                                #else
                                return  CrossPlatformInputManager.GetButton(input);
                                #endif
                            case InputButton.Down:
                                #if !CROSS_PLATFORM_INPUT
                                return Input.GetButtonDown(input);
                                #else
                                return  CrossPlatformInputManager.GetButtonDown(input);
                                #endif
                            case InputButton.Up:
                                #if !CROSS_PLATFORM_INPUT
                                return Input.GetButtonUp(input);
                                #else
                                return  CrossPlatformInputManager.GetButtonUp(input);
                                #endif
                        }
                        break;
                    case InputType.Key:
                        switch (GetPressed)
                        {
                            case InputButton.Press:
                                return Input.GetKey(key);
                            case InputButton.Down:
                                return Input.GetKeyDown(key);
                            case InputButton.Up:
                                return Input.GetKeyUp(key);
                        }
                        break;
                    default:
                        break;
                }
                return false;
            }
        }

        public void SetInputType(InputType type)
        {
            this.type = type;
        }

#region Constructors
        public InputRow(string i)
        {
            active = true;
            type = InputType.Input;
            input = i;
            GetPressed = InputButton.Down;
        }

        public InputRow(KeyCode k)
        {
            active = true;
            type = InputType.Key;
            key = k;
            GetPressed = InputButton.Down;
        }

        public InputRow(string i, KeyCode k)
        {
            active = true;
            type = InputType.Key;
            key = k;
            input = i;
            GetPressed = InputButton.Down;
        }

        public InputRow(string i, KeyCode k, InputButton pressed)
        {
            active = true;
            type = InputType.Key;
            key = k;
            input = i;
            GetPressed = InputButton.Down;
        }
        #endregion
    }
    #endregion
    public class MalbersInput : MonoBehaviour
    {
        private iMalbersInputs character;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private Transform m_Cam;
        public List<InputRow> inputs = new List<InputRow>();

        public string Horizontal = "Horizontal";
        public string Vertical = "Vertical";


        public bool cameraBaseInput;
        public bool alwaysForward;

        private float h;  //Horizontal Right & Left   Axis X
        private float v;  //Vertical   Forward & Back Axis Z

        void Awake()
        {
            //get the animalScript
            character = GetComponent<iMalbersInputs>();
        }

        private void Start()
        {
            if (Camera.main != null)   // get the transform of the main camera
                m_Cam = Camera.main.transform;
        }


        void OnDisable()
        {
            if (character != null) character.Move(Vector3.zero); //When the Input is Disable make sure the character/animal is not moving.
        }

        // Fixed update is called in sync with physics
        void Update()
        {
#if !CROSS_PLATFORM_INPUT
            h = Input.GetAxis(Horizontal);
            v = alwaysForward ? 1 : Input.GetAxis(Vertical);
#else
            h = CrossPlatformInputManager.GetAxis(Horizontal);
            v = alwaysForward ? 1 : CrossPlatformInputManager.GetAxis(Vertical);
#endif
            SetInput();
        }

      public virtual Vector3 CameraInputBased()
        {
            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 1, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
            return m_Move;
        }

        /// <summary>
        /// Send all the Inputs to the Animal
        /// </summary>
        protected virtual void SetInput()
        {
            if (cameraBaseInput)
            {
                character.Move(CameraInputBased());
            }
            else
            {
                character.Move(new Vector3(h, 0, v), false);
            }

            if (isActive("Attack1")) character.Attack1 = GetInput("Attack1");         //Get the Attack1 button
            if (isActive("Attack2")) character.Attack2 = GetInput("Attack2");         //Get the Attack1 button

            if (isActive("Action")) character.Action = GetInput("Action");  //Get the Action/Emotion button

            if (isActive("Jump")) character.Jump = GetInput("Jump");

            if (isActive("Shift")) character.Shift = GetInput("Shift");           //Get the Shift button

            if (isActive("Fly")) character.Fly = GetInput("Fly");                //Get the Fly button 
            if (isActive("Down")) character.Down = GetInput("Down");             //Get the Down button
            if (isActive("Dodge")) character.Dodge = GetInput("Dodge");             //Get the Down button

            if (isActive("Stun")) character.Stun = GetInput("Stun");             //Get the Stun button change the variable entry to manipulate how the stun works
            if (isActive("Death")) character.Death = GetInput("Death");            //Get the Death button change the variable entry to manipulate how the death works
            if (isActive("Damaged")) character.Damaged = GetInput("Damaged");

            if (isActive("Speed1"))     //Walk
            {
                bool s1 = GetInput("Speed1");
                if (character.Speed1 != s1) character.Speed1 = s1;
            }

            if (isActive("Speed2"))     //Trot
            {
                bool s2 = GetInput("Speed2");
                if (character.Speed2 != s2) character.Speed2 = s2;
            }

            if (isActive("Speed3"))     //Run
            {
                bool s3 = GetInput("Speed3");
                if (character.Speed3 != s3) character.Speed3 = s3;
            }

            //if (isActive("Speed2")) { character.Speed2 = GetInput("Speed2"); }              //Trot
            //if (isActive("Speed3")) { character.Speed3 = GetInput("Speed3"); }               //Run

            //Get the Death button change the variable entry to manipulate how the death works
        }

        /// <summary>
        /// Enable/Disable the Input
        /// </summary>
        public virtual void EnableInput(string inputName, bool value)
        {
            InputRow i = inputs.Find(item => item.name == inputName);

            if (i != null) i.active = value;
        }

        /// <summary>
        /// Thit will set the correct Input, from the Unity Input Manager or Keyboard.. you can always modify this code
        /// </summary>
        protected bool GetInput(string name)
        {
            // return inputs.Find(item => item.name.ToUpper() == name.ToUpper() && item.active); 

            foreach (InputRow item in inputs)
            {
                if (item.name.ToUpper() == name.ToUpper() && item.active)
                {
                    return item.GetInput;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the input is active
        /// </summary>
       public virtual bool isActive(string name)
        {
            // return inputs.Find(item => item.name == name).active;

            foreach (InputRow item in inputs)
            {
                if (item.name.ToUpper() == name.ToUpper()) return item.active;
            }
            return false;
        }
    }
}