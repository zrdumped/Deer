using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public InteractiveObjManager interactMgr;
    public AudioSource audioBGM;
    public AudioClip mainBGM;
    public AudioClip wakeUpBGM;
    public AudioClip dangerBGM;
    public AudioClip fightBGM;
    public AudioClip wildBGM;
    public AudioClip castleBGM;
    public MainPanelManager mainPanelMgr;
    public bool realTimeAmbientControl = false;
    public GameObject durabilityBar;
    [Range(0,5)]
    public float ambientIntensity;

    private int curChap; // Which chapter 
    private int curScene; // Which scene of current chapter 
    private bool isPause = false;
    private string curSceneStr = "";
    private bool isRunning = true; // Whether the game has been paused
    private InteractiveObjList interactiveObjList;
    private GameObject character;

    private TPPCamera cameraController;


	// Use this for initialization
	void Start () {
        GotoMainPanel();
	}
	
	// Update is called once per frame
	void Update () {

        if(realTimeAmbientControl == true) {
            RenderSettings.ambientIntensity = ambientIntensity;
        }

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
        if(Input.GetKeyDown(KeyCode.Alpha5)) {
            Switch2Scene(2, 0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)) {
            Switch2Scene(3, 0);
        }
        //if(Input.GetKeyDown(KeyCode.P)) {
        //    if(this.isRunning) {
        //        isRunning = false;
        //        PauseGame();
        //    }
        //    else {
        //        isRunning = true;
        //        ResumeGame();
        //    }
        //}
        //if(Input.GetKeyDown(KeyCode.Delete)) {
        //    ExitGame();
        //}
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(curSceneStr != "1_0" && curSceneStr != "0_0") {
                if(isRunning) {
                    isRunning = false;
                    ShowEscPanel();
                }
                else {
                    isRunning = true;
                    HideEscPanel();
                }
            }
        }


        if(character == null) {
            character = GameObject.FindWithTag("TppCharacter");
            // Set character into interactive object manager.
            if(character) {
                interactMgr.SetCharacter(character);
            }
        }
    }

    public void PauseGame() {
        //Time.timeScale = 0;
        StopCtrl();
    }

    public void ResumeGame() {
        //Time.timeScale = 1;
        StartCtrl();
    }

    public void ShowEscPanel() {
        mainPanelMgr.ShowEscPanel();
        PauseGame();
    }

    public void HideEscPanel() {
        mainPanelMgr.HideEscPanel();
        ResumeGame();
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
        mainPanelMgr.HideEscPanel();
        Switch2Scene(0, 0);
    }

    // Start a new game process and delete the record which is being stored
    public void NewGame() {
        // TODO: clear previously saved record
        Switch2Scene(1, 0);
        audioBGM.Stop();
    }

    public void ExitGame() {
        Application.Quit();
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
            // Show the main panel if in main panel scene.
            if(sceneName == "0_0") {
                mainPanelMgr.Reset();
                mainPanelMgr.ShowMainPanel();
            }
            else {
                mainPanelMgr.Reset();
            }


            // Find main camera and TPPSystem when new scene has been loaded.
            GameObject mainCam = GameObject.FindWithTag("MainCamera");
            if(mainCam) {
                cameraController = mainCam.GetComponent<TPPCamera>();
                character = GameObject.FindWithTag("TppCharacter");
            }
            if(cameraController) {
                Debug.Log("Find camera controller");
            }

            // Find Interacive Objects List whent new scene has been loaded.
            GameObject intObjList = GameObject.FindWithTag("IntObjList");
            if(intObjList) {
                interactiveObjList = intObjList.GetComponent<InteractiveObjList>();
            }
            if(interactiveObjList) {
                Debug.Log("Find interactive object list");
                interactMgr.SetIntObjList(interactiveObjList);
            }

        }

    }



    public void Switch2Scene(int targetChap, int targetScene) {
        if(interactMgr) {
            interactMgr.StopLooping();
        }
        string sceneName = "" + targetChap + "_" + targetScene;
        if(curSceneStr != "") {
            Unload(curSceneStr);
        }

        audioBGM.GetComponent<AudioSource>().volume = 0.12f;

        // some hard code
        if(targetChap == 2 && targetScene == 0) {
            sceneName = "Example_Night";
        }

        if(targetChap == 3 && targetScene == 0) {
            sceneName = "Demo_modify";
        }
        if(targetChap == 4 && targetScene == 0) {
            sceneName = "Epilogue";
        }
        if(targetChap == 1 && (targetScene == 3 || targetScene == 4)) {
            interactMgr.needTopLeftHint = true;
            interactMgr.hintArea.SetActive(true);
        }
        else {
            interactMgr.needTopLeftHint = false;
            interactMgr.hintArea.SetActive(false);
        }
        
        LoadAsync(sceneName);


        if(sceneName == "0_0" || sceneName == "1_0") {
            audioBGM.clip = mainBGM;
            if(curSceneStr != "0_0" && curSceneStr != "1_0") {
                audioBGM.Play();
            }
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
            mainPanelMgr.HideMainPanel(true); // Disable the logo Bloom Camera
        }
        else if(sceneName == "1_1") {
            audioBGM.clip = wakeUpBGM;
            if(curSceneStr == "0_0" || curSceneStr == "1_4") {
                audioBGM.Play();
            }
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
        }
        else if(sceneName == "1_4") {
            audioBGM.clip = dangerBGM;
            audioBGM.Play();
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
        }
        else if(sceneName == "Example_Night") {
            mainPanelMgr.HideMainPanel(true); // Disable the logo Bloom Camera
            audioBGM.clip = wildBGM;
            audioBGM.Play();
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
        }
        else if(sceneName == "Demo_modify") {
            mainPanelMgr.HideMainPanel(true); // Disable the logo Bloom Camera
            audioBGM.clip = castleBGM;
            audioBGM.Play();
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
        }
        else {
            audioBGM.clip = wakeUpBGM;
            if(curSceneStr == "0_0" || curSceneStr == "1_0" || curSceneStr == "1_4") {
                audioBGM.Play();
            }
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
        }
        curSceneStr = sceneName;

        if (sceneName == "1_3" || sceneName == "1_4")
        {
            UseBar();
        }
        else
        {
            durabilityBar.SetActive(false);
        }
    }

    public void PlayFightBGM() {
        audioBGM.clip = fightBGM;
        audioBGM.Play();
    }

    public void Switch2Scene(string sceneName)
    {
        Switch2Scene((int)sceneName[0] - 48, (int)sceneName[2] - 48);
    }

    public void DisplayBGM()
    {
        audioBGM.clip = wakeUpBGM;
        audioBGM.Play();
    }

    public void UseBar()
    {
        durabilityBar.SetActive(true);
        GameObject.Find("Character").GetComponent<Character>().UseBar();
    }
}
