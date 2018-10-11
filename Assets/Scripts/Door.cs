using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    //private bool active = false;
    [Header("Door")]
    public bool locked = true;
    public int unlockTorchNum = 4;
    [Header("Scene")]
    public string otherScene = "Scene Message";

	// Use this for initialization
	void Start () {
        //GameObject.Find("HintMessage").GetComponent<Text>().text = hintMessage;
        //active = false;
        if (PlayerPrefs.GetString("from") == otherScene)
        {
            PlayerPrefs.SetString("from", "");
            GameObject.Find("Character").transform.rotation = this.transform.Find("Placeholder").rotation;
            GameObject.Find("Character").transform.position = this.transform.Find("Placeholder").position;
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    public string TryOpen()
    {
        if (!locked)
        {
            PlayerPrefs.SetString("from", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(otherScene);
            return "Door has been opened";
        }
        else
            return "This door is locked";
    }

    public int LightTorchUp()
    {
        unlockTorchNum--;
        //GameObject.Find("OutputText").GetComponent<Text>().text += "Door locked.\n";
        if (unlockTorchNum == 0)
        {
            locked = false;
        }
        return unlockTorchNum;
    }
}
