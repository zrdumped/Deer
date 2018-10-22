using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DeerShowUpTrigger : MonoBehaviour {

    public GameObject player;
    public PlayableDirector timeline;
    public GameObject vmCam;

    private bool hasPlayed = false;
    private GameObject gm;
    private float timelineCounter = 0; // count time that the timeline has played
    private bool countingStart = false;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
        if(hasPlayed == false && player.transform.position.z < transform.position.z) {
            hasPlayed = true;
            vmCam.transform.position = Camera.main.transform.position;
            vmCam.transform.rotation = Camera.main.transform.rotation;
            if(gm) {
                gm.GetComponent<GameManager>().StopCtrl();
            }
            timeline.Play();
            countingStart = true;
        }
        if(countingStart == true ) {
            timelineCounter += Time.deltaTime;
            if(timelineCounter > timeline.duration) {
                countingStart = false;
                if(gm) {
                    gm.GetComponent<GameManager>().StartCtrl();
                }
            }
        }
	}
}
