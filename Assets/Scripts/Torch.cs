using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour {
    public bool active = false;
    public GameObject doorToUnlock;
    public int emissionRate = 50;

    public enum TorchType { Normal, Line, Explode};
    public TorchType type = TorchType.Normal;
    //line
    public GameObject lineTo;
    public float speed = 10;

    private ParticleSystem fire;
    private ParticleSystem.EmissionModule fireEmission;

    private GameObject trail;
    private GameObject trailParticleSystem;
    private ParticleSystem trailPS;
    private ParticleSystem.ShapeModule trailPSShape;

    private GameObject leadWire;

    // Use this for initialization
    void Start () {
        if (type == TorchType.Line)
        {
            trail = this.transform.Find("Trail").gameObject;

            fire = this.GetComponentInParent<ParticleSystem>();
            fireEmission = fire.emission;

            trailParticleSystem = this.transform.Find("TrailParticles").gameObject;
            trailPS = trailParticleSystem.GetComponent<ParticleSystem>();
            trailPSShape = trailPS.shape;
            trailPSShape.length = Vector3.Distance(this.transform.position, lineTo.transform.position);
            trailParticleSystem.transform.LookAt(lineTo.transform);
        }
    }

    // Update is called once per frame
    void Update () {
		//Trail
        if(type == TorchType.Line)
        {
            float step = speed * Time.deltaTime;
            trail.transform.position = Vector3.MoveTowards(trail.transform.position, lineTo.transform.position, step);
            if(trail.transform.position == lineTo.transform.position)
            {
                GameObject new_trail = Instantiate(trail, this.transform.position, this.transform.rotation, this.transform);
                trail = new_trail;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (active)
            return;
        if(type == TorchType.Line)
        {
            //set lead wire destination
            //light up
            //lead wire explodes and lights up fire when arriving
        }
        active = true;
        fireEmission.rateOverTime = emissionRate;
        int doorsNumToUnlock = doorToUnlock.GetComponent<Door>().LightTorchUp();
        GameObject.Find("OutputText").GetComponent<Board>().AddText("Torch Lit up! " +
            doorsNumToUnlock + " remains.");
        if(doorsNumToUnlock == 0)
            GameObject.Find("OutputText").GetComponent<Board>().AddText("Door unlocked.");
    }
}
