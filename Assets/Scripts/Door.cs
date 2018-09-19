using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    private bool active = false;
    [Header("Enter")]
    public GameObject placeHolder;
    [Header("Scene")]
    public string currentScene = "Scene Message";
    public string otherScene = "Scene Message";
    [Header("Interaction")]
    public string hintMessage = "Hint Message";
    public KeyCode enterKey = KeyCode.E;

	// Use this for initialization
	void Start () {
        //GameObject.Find("HintMessage").GetComponent<Text>().text = hintMessage;
        active = false;
        if(PlayerPrefs.GetString("from") == otherScene)
        {
            PlayerPrefs.SetString("from", "");
            GameObject.Find("Liam").transform.position = placeHolder.transform.position;
            GameObject.Find("Liam").transform.rotation = placeHolder.transform.rotation;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (active && Input.GetKeyDown(enterKey))
        {
            PlayerPrefs.SetString("from", currentScene);
            SceneManager.LoadScene(otherScene);
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
