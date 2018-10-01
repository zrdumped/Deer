using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {
    public bool active = false;
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
        active = true;
        emission.rateOverTime = 30;
    }
}
