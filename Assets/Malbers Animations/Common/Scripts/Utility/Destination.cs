using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour {
    GameObject target;
    // Use this for initialization
    void Start () {
        target = GameObject.Find("DeerFriendly (1)");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in");
        //GameObject.Find("DeerFriendly (1)").GetComponent<MalbersAnimations.AIDeer>().enabled = false;
        target.transform.rotation = new Quaternion(
           target.transform.rotation.x,
           180,
           target.transform.rotation.z,
           target.transform.rotation.w);
        GetComponent<Destination>().enabled = false;
    }
}
