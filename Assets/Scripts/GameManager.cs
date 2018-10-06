using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    private int curChap; // Which chapter 
    private int curScene; // Which scene of current chapter 
    private bool isPause = false;
    private string curSceneStr = "";


	// Use this for initialization
	void Start () {
        GotoMainPanel();
	}
	
	// Update is called once per frame
	void Update () {
        // For Test ONLY
        if(Input.GetKeyDown(KeyCode.H)) {
            Switch2Scene(0, 0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0)) {
            Switch2Scene(1, 0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            Switch2Scene(1, 1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            Switch2Scene(1, 2);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            Switch2Scene(1, 3);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)) {
            Switch2Scene(1, 4);
        }
    }

    public void PauseGame() {

    }

    public void ResumeGame() {

    }

    // Enable player to control the character
    public void StartCtrl() {

    }

    // Disable player to control the character
    public void StopCtrl() {

    }

    public void GotoMainPanel() {
        Switch2Scene(0, 0);
    }

    // Start a new game process and delete the record which is being stored
    public void NewGame() {
        // TODO: clear previously saved record


        Switch2Scene(1, 0);
    }

    public void ExitGame() {

    }

    // Save the player's current game process(ONLY ONE record is supported)
    public void SaveProcess() {

    }

    public void LoadProcess() {

    }

    public void Load(string sceneName) {
        if(!SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void Unload(string sceneName) {
        if(SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.UnloadSceneAsync(sceneName);
    }

    public void Switch2Scene(int targetChap, int targetScene) {
        string sceneName = "" + targetChap + "_" + targetScene;
        if(curSceneStr != "") {
            Unload(curSceneStr);
        }
        Load(sceneName);
        curSceneStr = sceneName;
    }
}
