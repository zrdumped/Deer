using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputListener : MonoBehaviour {

    bool active = false;
    public string messageOnActive;
    private string messageOnTrigger;

    public KeyCode triggerKey = KeyCode.E;
	public GameObject character = null;
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
                messageOnTrigger = this.GetComponent<Key>().TriggerShow();
            }
            GameObject.Find("HintMessage").GetComponent<Text>().text = messageOnTrigger;
        }
        //Debug
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (this.GetComponent<Key>() != null)
            {
                messageOnTrigger = this.GetComponent<Key>().activate();
            }
            GameObject.Find("HintMessage").GetComponent<Text>().text = messageOnTrigger;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Character")
            return;
        active = true;
		this.character = other.gameObject;
        GameObject.Find("HintMessage").GetComponent<Text>().text = messageOnActive;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != "Character")
            return;
        active = false;
		this.character = null;
		GameObject.Find("HintMessage").GetComponent<Text>().text = "";
    }
}
