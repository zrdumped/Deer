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
    //public float LightBallChangeRate = 0.01f;
    //public float PointLightRangeChangeRate = 1.0f;
    //public float PointLightIntensityChangeRate = 1.0f;
    //public float maxLightBallScale = 0.15f;
    //private float currentLightBallChangeRate;
    public float maxLightBallScale = 0.15f;
    public float maxLightIntensity = 2;
    public float maxLightRange = 9;
    [Header("Control By Keyboard")]
    public float changeTimes = 10;
    [Header("Keyboard Control Config")]
    public KeyCode inflateKey = KeyCode.Z;
    public KeyCode deflateKey = KeyCode.X;
    public KeyCode disappearKey = KeyCode.C;
    //private enum toolState { Inflate, Deflate, Inactive };
    //private toolState state = toolState.Inactive;
    private GameObject lightBall;
    private GameObject lightSphere;

    private float LightBallScaleRate;
    private float LightIntensityRate;
    private float LightRangeRate;

    void Start () {
        lightBall = this.transform.Find("LightBall").gameObject;
        lightBall.transform.localScale = new Vector3(0, 0, 0);

        lightSphere = this.transform.Find("Point Light").gameObject;
        lightSphere.GetComponent<Light>().range = 0;
        lightSphere.GetComponent<Light>().intensity = 0;

        if (!controlByVoice)
        {
            LightBallScaleRate = maxLightBallScale / changeTimes;
            LightIntensityRate = maxLightIntensity / changeTimes;
            LightRangeRate = maxLightRange / changeTimes;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //LightBall.transform.position = this.transform.position + new Vector3(0, yOffset, 0);
        //Control By Keyboard
        if (!controlByVoice)
        {
            if (Input.GetKey(inflateKey))
            {
                //Debug.Log(lightBall.transform.localScale.x + LightBallChangeRate);
                //Debug.Log(maxLightBallScale);
                if (lightBall.transform.localScale.x + LightBallScaleRate < maxLightBallScale)
                {
                    lightBall.transform.localScale += new Vector3(LightBallScaleRate, LightBallScaleRate, LightBallScaleRate);
                    lightSphere.GetComponent<Light>().range += LightRangeRate;
                    lightSphere.GetComponent<Light>().intensity += LightIntensityRate;
                }
            }
            else if (Input.GetKey(deflateKey))
            {
                if (lightBall.transform.localScale.x - LightBallScaleRate >= 0)
                {
                    lightBall.transform.localScale -= new Vector3(LightBallScaleRate, LightBallScaleRate, LightBallScaleRate);
                    lightSphere.GetComponent<Light>().range -= LightRangeRate;
                    lightSphere.GetComponent<Light>().intensity -= LightIntensityRate;
                }
                else
                {
                    lightBall.transform.localScale = new Vector3(0, 0, 0);
                    lightSphere.GetComponent<Light>().range = 0;
                    lightSphere.GetComponent<Light>().intensity = 0;
                }
            }
            else if (Input.GetKeyDown(disappearKey))
            {
                lightBall.transform.localScale = new Vector3(0, 0, 0);
                lightSphere.GetComponent<Light>().range = 0;
                lightSphere.GetComponent<Light>().intensity = 0;
            }
        }
    }
}
