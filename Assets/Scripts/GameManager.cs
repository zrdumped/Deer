using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    private int curChap; // Which chapter 
    private int curScene; // Which scene of current chapter 
    private bool isPause = false;


	// Use this for initialization
	void Start () {
        Load("1_0_Prologue");
	}
	
	// Update is called once per frame
	void Update () {
		
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

    }

    // Start a new game process and delete the record which is being stored
    public void NewGame() {

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

    }
}
