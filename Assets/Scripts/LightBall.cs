using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBall : MonoBehaviour {
    [Header("State Config")]
    public bool controlByVoice = false;
    public float LightBallChangeRate = 0.01f;
    private float currentLightBallChangeRate;
    [Header("Keyboard Control Config")]
    public KeyCode inflateKey = KeyCode.Z;
    public KeyCode deflateKey = KeyCode.X;
    private enum toolState { Inflate, Deflate, Inactive };
    private toolState state = toolState.Inactive;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Control By Keyboard
        if (Input.GetKeyDown(inflateKey) && !controlByVoice && state == toolState.Inactive)
        {
            state = toolState.Inflate;
            currentLightBallChangeRate = LightBallChangeRate;
        }

        if (Input.GetKeyUp(inflateKey) && !controlByVoice && state == toolState.Inflate)
        {
            state = toolState.Inactive;
            currentLightBallChangeRate = LightBallChangeRate;
        }

        if (Input.GetKeyDown(deflateKey) && !controlByVoice && state == toolState.Inactive)
        {
            state = toolState.Deflate;
        }

        if (Input.GetKeyUp(deflateKey) && !controlByVoice && state == toolState.Deflate)
        {
            state = toolState.Inactive;
        }

        //Logic
        if (state == toolState.Inflate)
        {
            this.transform.localScale += new Vector3(LightBallChangeRate, LightBallChangeRate, LightBallChangeRate);
            currentLightBallChangeRate += LightBallChangeRate;
        }
        else if (state == toolState.Deflate)
        {
            this.transform.localScale -= new Vector3(LightBallChangeRate, LightBallChangeRate, LightBallChangeRate);
            currentLightBallChangeRate += LightBallChangeRate;
        }
    }

}
