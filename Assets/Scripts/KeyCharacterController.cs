using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Playables;

// 这个类用于实现玩家对着key按E之后角色自动走到和key重合
// 也用于在key成功激活之后启动timeline

public class KeyCharacterController : MonoBehaviour {

    
    public GameObject vmCam;

    private PlayableDirector timeline;
    private GameObject character;
    private ThirdPersonCharacter tppC;
    private GameObject gm;
    

    private bool attracting = false;
    private bool countingStart = false;
    private float timelineCounter = 0; // count time that the timeline has played

    // Use this for initialization
    void Start () {
        timeline = gameObject.GetComponent<PlayableDirector>();
        character = GameObject.FindWithTag("TppCharacter");
        tppC = character.GetComponent<ThirdPersonCharacter>();
        gm = GameObject.Find("GameManager");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(attracting == true) {
            if(Vector3.Distance(character.transform.position, transform.position) > 1.3) {
               
                Vector3 direct = transform.position - character.transform.position;
                direct.y = 0;
                direct = direct.normalized;
                tppC.Move(direct, false, false);
            }
            else {
                if(gm) {
                    gm.GetComponent<GameManager>().StopMoveCtrl();
                }
            }
        }
	}

    void Update() {
        // Fot Test ONLY
        if(Input.GetKeyDown(KeyCode.R)) {
            PlayTimeline();
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            GotoKey();
        }
        

        if(countingStart == true) {
            timelineCounter += Time.deltaTime;
            if(timelineCounter > timeline.duration) {
                countingStart = false;
                if(gm) {
                    gm.GetComponent<GameManager>().StartCtrl();
                }
            }
        }
    }

    public void GotoKey() {
        attracting = true;
    }

    public void PlayTimeline() {
        if(gm) {
            gm.GetComponent<GameManager>().StartMoveCtrl();
        }
        attracting = false;
        vmCam.transform.position = Camera.main.transform.position;
        vmCam.transform.rotation = Camera.main.transform.rotation;
        if(gm) {
            gm.GetComponent<GameManager>().StopCtrl();
        }
        timeline.Play();
        countingStart = true;
    }
}
