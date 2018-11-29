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
        if (other.name == "LightBallMesh")
        {
            this.GetComponentInParent<AlertController>().Alert();
        }
        else if (other.name == "Character" && this.GetComponentInParent<AlertController>().GetStatus() == AlertController.AlertTriggerStatus.Alert)
        {
            Debug.Log("Game Over");
            GameObject.Find("GameManager").GetComponent<GameManager>().tobeDead = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().Switch2Scene(1, 4);
        }
        //status = AlertTriggerStatus.Alert;
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.name == "LightBallMesh")
    //        this.GetComponentInParent<AlertController>().Patrol();
    //    //status = AlertTriggerStatus.Missing;
    //}
}
