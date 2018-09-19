using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    public bool active = false;
    public string sceneMessage = "Scene Message";
    public string hintMessage = "Hint Message";
    public string leadTo = "";
    public KeyCode enterKey = KeyCode.E;

	// Use this for initialization
	void Start () {
        GameObject.Find("HintMessage").GetComponent<Text>().text = sceneMessage;
    }
	
	// Update is called once per frame
	void Update () {
        if (active && Input.GetKeyDown(enterKey))
        {
            if (leadTo != "")
                SceneManager.LoadScene(leadTo);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        active = true;
        GameObject.Find("HintMessage").GetComponent<Text>().text = hintMessage;
    }

    private void OnTriggerExit(Collider other)
    {
        active = false;
        GameObject.Find("HintMessage").GetComponent<Text>().text = sceneMessage;
    }
}
