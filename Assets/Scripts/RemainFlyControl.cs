using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemainFlyControl : MonoBehaviour {

    public GameObject target;
    public float lerpVal = 0;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 curPos = transform.position;
        Vector3 tmpPos = Vector3.Lerp(curPos, target.transform.position, lerpVal);
        transform.position = tmpPos;
	}
}
