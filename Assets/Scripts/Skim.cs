using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skim : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
            "The playable contents in this scene is under development", -1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
