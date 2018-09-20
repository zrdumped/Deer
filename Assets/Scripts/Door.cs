using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    private bool active = false;
    [Header("Door")]
    public bool locked = false;
    [Header("Scene")]
    public string otherScene = "Scene Message";
    [Header("Interaction")]
    public string hintMessage = "Hint Message";
    public KeyCode enterKey = KeyCode.E;

	// Use this for initialization
	void Start () {
        //GameObject.Find("HintMessage").GetComponent<Text>().text = hintMessage;
        active = false;
        if (PlayerPrefs.GetString("from") == otherScene)
        {
            PlayerPrefs.SetString("from", "");
            GameObject.Find("Character").transform.rotation = this.transform.Find("Placeholder").rotation;
            GameObject.Find("Character").transform.position = this.transform.Find("Placeholder").position;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (active && Input.GetKeyDown(enterKey))
        {
            if (!locked)
            {
                PlayerPrefs.SetString("from", SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(otherScene);
            }
            else
                GameObject.Find("HintMessage").GetComponent<Text>().text = "This door is locked";
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
        GameObject.Find("HintMessage").GetComponent<Text>().text = "";
    }
}
