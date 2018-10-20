using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour {
    public enum KeyStatus { Inactive, OnShow, Active};
    public GameObject InactiveBall;
    public GameObject ActiveBall;
    public Light[] Lights;
    public int OnLitFrames = 200;

    private float[] lightRangeSteps, lightIntensitySteps;
    private int curOnLitFrame;

    private KeyStatus status = KeyStatus.Inactive;

    // Use this for initialization
    void Start () {
        InactiveBall.GetComponent<ParticleSystem>().Play(true);
        ActiveBall.GetComponent<ParticleSystem>().Stop(true);
        lightRangeSteps = new float[Lights.Length];
        lightIntensitySteps = new float[Lights.Length];
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
	void FixedUpdate ()
    {
        while (curOnLitFrame < OnLitFrames)
        {
            curOnLitFrame++;
            int id = 0;
            foreach (Light light in Lights)
            {
                //light.GetComponent<Light>().enabled = false;
                light.GetComponent<Light>().range += lightRangeSteps[id];
                light.GetComponent<Light>().intensity += lightIntensitySteps[id];
                id++;
            }
            return;
        }
    }

    public string TriggerShow()
    {
        if(status == KeyStatus.Inactive)
        {
            InactiveBall.GetComponent<ParticleSystem>().Stop(true);
            this.GetComponentInChildren<Pattern>().StartHint();
            //walk on stage and disable moving
            status = KeyStatus.OnShow;
            return "Press E to stop. Match the spheres with your voice";
        }
        else if(status == KeyStatus.OnShow)
        {
            InactiveBall.GetComponent<ParticleSystem>().Play(true);
            this.GetComponentInChildren<Pattern>().StopHint();
            //walk off stage and enable moving
            status = KeyStatus.Inactive;
            return "Press E to dispaly.";
        }
        else
        {
            return "";
        }
    }

    public string activate()
    {
        this.GetComponentInChildren<Pattern>().StopHint();
        status = KeyStatus.Active;
        ActiveBall.GetComponent<ParticleSystem>().Play(true);
        curOnLitFrame = 0;
        foreach (Light light in Lights)
        {
            light.GetComponent<Light>().enabled = true;
        }
        return "Activated";
    }
}
