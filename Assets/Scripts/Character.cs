using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    //[Header("Light Ball Settings")]
    //public float yOffset = 0.847f;
    //public GameObject LightBall;
    // Use this for initialization
    [Header("State Config")]
    public bool controlByVoice = false;
    public float LightBallChangeRate = 0.01f;
    private float currentLightBallChangeRate;
    [Header("Keyboard Control Config")]
    public KeyCode inflateKey = KeyCode.Z;
    public KeyCode deflateKey = KeyCode.X;
    public KeyCode disappearKey = KeyCode.C;
    //private enum toolState { Inflate, Deflate, Inactive };
    //private toolState state = toolState.Inactive;
    private GameObject lightBall;

    void Start () {
        lightBall = this.transform.Find("LightBall").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        //LightBall.transform.position = this.transform.position + new Vector3(0, yOffset, 0);
        //Control By Keyboard
        if (!controlByVoice)
        {
            if (Input.GetKey(inflateKey))
                lightBall.transform.localScale += new Vector3(LightBallChangeRate, LightBallChangeRate, LightBallChangeRate);
            else if (Input.GetKey(deflateKey))
                lightBall.transform.localScale -= new Vector3(LightBallChangeRate, LightBallChangeRate, LightBallChangeRate);
            else if (Input.GetKeyDown(disappearKey))
                lightBall.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}
