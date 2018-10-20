using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBall : MonoBehaviour {
	public SoundDetector sound;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
		GetComponent<Light>().range = sound.midi / 20;
		GetComponent<Light>().intensity = sound.midi / 20;
    }
}
