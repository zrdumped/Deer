using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour {
    public string lightBallName = "LightBall";
    public string hintBallName = "HintsBall";

    private bool hintBallActive = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hintBallActive && Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("E" + this.name);
            if (this.GetComponentInChildren<Pattern>().onShow)
            {
                this.GetComponentInChildren<Pattern>().StopHint();
                GameObject.Find("HintMessage").GetComponent<Text>().text = "Press 'E' to Display";
            }
            else
            {
                this.GetComponentInChildren<Pattern>().StartHint();
                GameObject.Find("HintMessage").GetComponent<Text>().text = "Press 'E' to Stop";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == lightBallName)
        {
            this.GetComponentInChildren<Pattern>().enabled = true;
            GameObject.Find("HintMessage").GetComponent<Text>().text = "Press 'E' to Display";
            hintBallActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == lightBallName)
        {
            this.GetComponentInChildren<Pattern>().StopHint();
            this.GetComponentInChildren<Pattern>().enabled = false;
            hintBallActive = false;
        }
    }
}
