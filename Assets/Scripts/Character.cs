using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("Durability Config")]
    public Slider durabilityBar;
    public float maxDecayVelocity = 3;
    public float durabilityRecoverVelocityNormal = 0.7f;
    public float durabilityRecoverVelocityEmerge = 0.1f;
    public float emergeValue = 30;
    public float maxDurability = 100;
    public Color normalColor;
    public Color EmergeColor;
    public Image BarFill;
    [Header("Keyboard Control Config")]
    public float changeTimes = 10;
    public KeyCode inflateKey = KeyCode.Z;
    public KeyCode deflateKey = KeyCode.X;
    public KeyCode disappearKey = KeyCode.C;
	[Header("Sound Detection and Calculation")]
	public SoundDetector soundDetector;
	public SoundCalcMethodType calcMethod = SoundCalcMethodType.NORMAL;
	public float soundValueAfterCalc;
    //private enum toolState { Inflate, Deflate, Inactive };
    //private toolState state = toolState.Inactive;
    private GameObject lightBall;
    private GameObject lightSphere;

    private float LightBallScaleRate;
    private float LightIntensityRate;
    private float LightRangeRate;

    private float durability;

    void Start () {
        lightBall = this.transform.Find("LightBall").gameObject;
        lightBall.transform.localScale = new Vector3(0, 0, 0);

        lightSphere = this.transform.Find("Point Light").gameObject;
        lightSphere.GetComponent<Light>().range = 0;
        lightSphere.GetComponent<Light>().intensity = 0;

        if (!controlByVoice)
        {
			Debug.Log("using keyboard!");
            LightBallScaleRate = maxLightBallScale / changeTimes;
            LightIntensityRate = maxLightIntensity / changeTimes;
            LightRangeRate = maxLightRange / changeTimes;
        }

        durability = maxDurability;
    }
	
	// Update is called once per frame
	void Update () {
        if (!controlByVoice && durability > emergeValue)
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
    else {
			AdjustLightBallWithSound();
		}
        //recover
        if(lightBall.transform.localScale.x == 0)
        {
            float recoverVelocity;
            if (durability < emergeValue)
                recoverVelocity = durabilityRecoverVelocityEmerge;
            else
                recoverVelocity = durabilityRecoverVelocityNormal;

            if (durability + recoverVelocity >= maxDurability)
                durability = maxDurability;
            else
                durability += recoverVelocity;
        }
        //decay
        else
        {
            /*float durabilityDecayVelocity = (lightBall.transform.localScale.x / maxLightBallScale) * maxDecayVelocity;
            if (durability - durabilityDecayVelocity >= 0)
                durability -= durabilityDecayVelocity;
            else
            {
                durability = 0;
                lightBall.transform.localScale = new Vector3(0, 0, 0);
                lightSphere.GetComponent<Light>().range = 0;
                lightSphere.GetComponent<Light>().intensity = 0;
            }*/
        }
        durabilityBar.value = durability;
        //Debug.Log(durability);
        //Debug.Log(normalColor.ToString());
        if (durability < emergeValue)
            BarFill.color = EmergeColor;
        else
            BarFill.color = normalColor;
    }

	void AdjustLightBallWithSound() {
		int midi = soundDetector.midi;
		soundValueAfterCalc = SoundCalculator.calc(midi, calcMethod);
		//Debug.Log(midi+", "+scale);
		float lightBallScale = soundValueAfterCalc * maxLightBallScale;
		float lightIntensity = soundValueAfterCalc * maxLightIntensity;
		float lightRange = soundValueAfterCalc * maxLightRange;
		lightBall.transform.localScale = new Vector3(lightBallScale, lightBallScale, lightBallScale);
		lightSphere.GetComponent<Light>().range = lightRange;
		lightSphere.GetComponent<Light>().intensity = lightIntensity;
	}
}
