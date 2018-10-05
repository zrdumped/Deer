using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torch : MonoBehaviour {
    public bool active = false;
    public GameObject doorToUnlock;
    public int emissionRate = 50;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule emission;

	// Use this for initialization
	void Start () {
        ps = this.GetComponentInParent<ParticleSystem>();
        emission = ps.emission;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (active)
            return;
        active = true;
        emission.rateOverTime = emissionRate;
        int doorsNumToUnlock = doorToUnlock.GetComponent<Door>().LightTorchUp();
        GameObject.Find("OutputText").GetComponent<Board>().AddText("Torch Lit up! " +
            doorsNumToUnlock + " remains.");
        if(doorsNumToUnlock == 0)
            GameObject.Find("OutputText").GetComponent<Board>().AddText("Door unlocked.");
    }
}
