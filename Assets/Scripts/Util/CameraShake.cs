using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    [Range(0, 0.3f)]
    public float xRange;

    [Range(0, 0.3f)]
    public float yRange;

    public float speed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Mathf.Sin(Time.time * speed) * transform.right * xRange;
        transform.position += Mathf.Cos(Time.time * speed) * transform.up * yRange;
	}
}
