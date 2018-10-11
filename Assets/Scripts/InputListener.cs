using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputListener : MonoBehaviour {

    bool active = false;
    public string messageOnActive;
    private string messageOnTrigger;

    public KeyCode triggerKey = KeyCode.E;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!active)
            return;
        if (Input.GetKeyDown(triggerKey))
        {
            if (this.GetComponent<Door>() != null)
            {
                messageOnTrigger = this.GetComponent<Door>().TryOpen();
            }
            else if(this.GetComponent<Key>() != null)
            {
                if (this.GetComponentInChildren<Pattern>().onShow)
                {
                    messageOnTrigger = this.GetComponentInChildren<Pattern>().StopHint();
                }
                else
                {
                    messageOnTrigger = this.GetComponentInChildren<Pattern>().StartHint();
                }
            }
            GameObject.Find("HintMessage").GetComponent<Text>().text = messageOnTrigger;
        }
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Character")
            return;
        active = true;
        GameObject.Find("HintMessage").GetComponent<Text>().text = messageOnActive;
        if (this.GetComponent<Key>() != null)
        {
            this.GetComponentInChildren<Pattern>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != "Character")
            return;
        active = false;
        GameObject.Find("HintMessage").GetComponent<Text>().text = "";
        if (this.GetComponent<Key>() != null)
        {
            this.GetComponentInChildren<Pattern>().StopHint();
            this.GetComponentInChildren<Pattern>().enabled = false;
        }
    }
}
