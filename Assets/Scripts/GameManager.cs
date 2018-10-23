using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    private int curChap; // Which chapter 
    private int curScene; // Which scene of current chapter 
    private bool isPause = false;
    private string curSceneStr = "";
    private bool isRunning = true; // Whether the game has been paused

    private TPPCamera cameraController;


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
        if(Input.GetKeyDown(KeyCode.P)) {
            if(this.isRunning) {
                isRunning = false;
                PauseGame();
            }
            else {
                isRunning = true;
                ResumeGame();
            }
        }
        if(Input.GetKeyDown(KeyCode.Delete)) {
            ExitGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
        StopCtrl();
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        StartCtrl();
    }

    // Enable player to control the character
    public void StartCtrl() {
        if(cameraController) {
            cameraController.EnableCtrl();
        }
    }

    // Disable player to control the character
    public void StopCtrl() {
        if(cameraController) {
            cameraController.DisableCtrl();
        }
    }

    // Enable player to control the character's movement
    public void StartMoveCtrl() {
        if(cameraController) {
            cameraController.EnableMove();
        }
    }

    // Disable player to control the character's movement(but can cantrol view)
    public void StopMoveCtrl() {
        if(cameraController) {
            cameraController.DisableMove();
        }
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
        Application.Quit();
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

    public void LoadAsync(string sceneName) {
        StartCoroutine(LoadSceneJob(sceneName));
    }

    IEnumerator LoadSceneJob(string sceneName) {
        //if(!SceneManager.GetSceneByName(sceneName).isLoaded) {
            

        //}
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while(!asyncLoad.isDone) {
            yield return null;
        }

        if(SceneManager.GetSceneByName(sceneName).isLoaded) {
            GameObject mainCam = GameObject.FindWithTag("MainCamera");
            if(mainCam) {
                cameraController = mainCam.GetComponent<TPPCamera>();
            }
            if(cameraController) {
                Debug.Log("Find camera controller");
            }
        }

    }



    public void Switch2Scene(int targetChap, int targetScene) {
        string sceneName = "" + targetChap + "_" + targetScene;
        if(curSceneStr != "") {
            Unload(curSceneStr);
        }
        //Load(sceneName);
        LoadAsync(sceneName);
        curSceneStr = sceneName;
    }
}
