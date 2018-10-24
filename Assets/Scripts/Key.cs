using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
	public enum KeyStatus { Inactive, OnShow, Active };
	[Header("VFX")]
	//the vfx shown normally
	public GameObject InactiveBall;
	//the vfx shown after pressing E
	public GameObject HintBall;
	//the vfx shown after all matching
	public GameObject ActiveBall;
	//the vfxs shown after single matching
	public GameObject AlertBall;
	public GameObject ConfirmBall;
	public int DestroySeconds = 1;
	[Header("Light Up Settings")]
	public Light[] Lights;
	public float initEmissionScale = 1;
	public Material LightMaterial;
	public int OnLitFrames = 200;


	private float[] lightRangeSteps, lightIntensitySteps;
	private float lightEmissionScaleStep;
	private int curOnLitFrame;
	private KeyStatus status = KeyStatus.Inactive;
    private int curMatchNum = 0;
    private int targetMatchNum;

	// Use this for initialization
	void Start()
	{
		InactiveBall.GetComponent<ParticleSystem>().Play(true);
		ActiveBall.GetComponent<ParticleSystem>().Stop(true);
		lightRangeSteps = new float[Lights.Length];
		lightIntensitySteps = new float[Lights.Length];
		lightEmissionScaleStep = initEmissionScale / (float)OnLitFrames;
		//Debug.Log(lightEmissionScaleStep);
		LightMaterial.SetFloat("_EMISSION", 0);
		int id = 0;
		foreach (Light light in Lights)
		{
			lightRangeSteps[id] = light.GetComponent<Light>().range / (float)OnLitFrames;
			light.GetComponent<Light>().range = 0;
			lightIntensitySteps[id] = light.GetComponent<Light>().intensity / (float)OnLitFrames;
			light.GetComponent<Light>().intensity = 0;
			light.GetComponent<Light>().enabled = false;
			id++;
		}
		curOnLitFrame = OnLitFrames;
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (curOnLitFrame < OnLitFrames)
		{
			curOnLitFrame++;
			int id = 0;
			LightMaterial.SetFloat("_EMISSION", LightMaterial.GetFloat("_EMISSION") + lightEmissionScaleStep);
			foreach (Light light in Lights)
			{
				//light.GetComponent<Light>().enabled = false;
				light.GetComponent<Light>().range += lightRangeSteps[id];
				light.GetComponent<Light>().intensity += lightIntensitySteps[id];
				id++;
			}
		}
	}

	public string TriggerShow()
	{
		if (status == KeyStatus.Inactive)
		{
			InactiveBall.SetActive(false);
            HintBall.SetActive(true);
            this.GetComponentInChildren<Pattern>().enabled = true;
            targetMatchNum = this.GetComponentInChildren<Pattern>().pattern.Length;
            //walk on stage and disable moving
            status = KeyStatus.OnShow;
			//GetComponentInChildren<Pattern>().character = GetComponent<InputListener>().character;
			return "Press E to stop. Match the spheres with your voice";
		}
		else if (status == KeyStatus.OnShow)
		{
            HintBall.SetActive(false);
            InactiveBall.SetActive(true);
            //InactiveBall.GetComponent<ParticleSystem>().Play(true);
            //this.GetComponentInChildren<Pattern>().enabled = false;
			//walk off stage and enable moving
			status = KeyStatus.Inactive;
			//GetComponentInChildren<Pattern>().character = null;
			return "Press E to dispaly.";
		}
		else
		{
			return "";
		}
	}

	public void activate()
	{
		Debug.Log("called activated");
		this.GetComponentInChildren<Pattern>().enabled = false;
        HintBall.SetActive(false);
        status = KeyStatus.Active;
        //ActiveBall.GetComponent<ParticleSystem>().Play(true);
        ActiveBall.SetActive(true);
        curOnLitFrame = 0;
		foreach (Light light in Lights)
		{
			light.GetComponent<Light>().enabled = true;
		}
        GameObject.Find("HintMessage").GetComponent<Text>().text = "Activated";
        //return "Activated";
    }

	public void MatchSucceed()
	{
		GameObject tmp_ConfirmBall = Instantiate(ConfirmBall, this.transform);
		tmp_ConfirmBall.transform.position = HintBall.transform.position;
		tmp_ConfirmBall.transform.localScale = HintBall.transform.localScale;
		//HintBall.transform.localScale = new Vector3(0, 0, 0);
		//Debug.Log("haha");
		tmp_ConfirmBall.SetActive(true);
		StartCoroutine(WaitAndDestroy(tmp_ConfirmBall, DestroySeconds));
        curMatchNum ++;

        return;
	}

	public void MatchFail()
	{
		GameObject tmp_ConfirmBall = Instantiate(AlertBall, this.transform);
		tmp_ConfirmBall.transform.position = HintBall.transform.position;
		tmp_ConfirmBall.transform.localScale = HintBall.transform.localScale;
		//HintBall.transform.localScale = new Vector3(0, 0, 0);
		//Debug.Log("haha");
		tmp_ConfirmBall.SetActive(true);
		StartCoroutine(WaitAndDestroy(tmp_ConfirmBall, DestroySeconds, false));
		return;
	}

    public void testAllMatch()
    {
        if (curMatchNum == targetMatchNum)
            activate();
        else
            GameObject.Find("HintMessage").GetComponent<Text>().text = "You have to match all the pitches to activate it";
    }

	IEnumerator WaitAndDestroy(GameObject obj, int time, bool match = true)
	{
		yield return new WaitForSeconds(time);
		//HintBall.transform.localScale = obj.transform.localScale;
		if (!match)
			HintBall.transform.localScale = new Vector3(0, 0, 0);
		Destroy(obj);
	}

    public void MatchReset()
    {
        curMatchNum = 0;
    }
}
