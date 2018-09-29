using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisSelfRotating : MonoBehaviour {

    public GameObject obj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        obj.transform.Rotate(Vector3.forward*Time.deltaTime * 10.0f);
	}
}
