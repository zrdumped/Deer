using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skim : MonoBehaviour {
    public string text = "The playable contents in this scene is under development";
    public bool playable = false;

    // Use this for initialization
    void Start () {
        if (!playable)
        {
            GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
               text, -1);
        }
        else
        {
            if (GameObject.Find("GameManager").GetComponent<GameManager>().tobeDead)
            {
                GameObject.Find("InteractiveObjManager").GetComponent<InteractiveObjManager>().SetTextTimed(
                    text, 4);
                GameObject.Find("GameManager").GetComponent<GameManager>().tobeDead = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
