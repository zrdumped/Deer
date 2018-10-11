using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.name == "LightBall")
            this.GetComponentInParent<AlertController>().Alert();
        //status = AlertTriggerStatus.Alert;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "LightBall")
            this.GetComponentInParent<AlertController>().Patrol();
        //status = AlertTriggerStatus.Missing;
    }
}
