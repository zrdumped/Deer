﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RemainPickupTrigger : MonoBehaviour {

    public GameObject player;
    public float threshodDistance = 10;
    public PlayableDirector timeline;
    public GameObject vmCam;
    public GameObject Book;
    public GameObject StarPlatinum;

    private bool hasPlayed = false;
    private GameObject gm;
    private float timelineCounter = 0; // count time that the timeline has played
    private bool countingStart = false;
    private Vector3 recordPosition;
    private Quaternion recordRotation;

    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager");
    }
	
	// Update is called once per frame
	void Update () {
        if(hasPlayed == false && Vector3.Distance(player.transform.position, transform.position) < threshodDistance) {
            hasPlayed = true;
            vmCam.transform.position = Camera.main.transform.position;
            vmCam.transform.rotation = Camera.main.transform.rotation;
            if(gm) {
                gm.GetComponent<GameManager>().StopCtrl();
            }
            Vector3 pos = transform.position;
            pos.y = player.transform.position.y;
            player.transform.LookAt(pos);
            StandShow();
            timeline.Play();
            countingStart = true;
        }

        if(countingStart == true) {
            timelineCounter += Time.deltaTime;
            if(timelineCounter > timeline.duration) {
                Debug.Log("timeline is finished.");
                countingStart = false;
                StandRetrieve();
                if(gm) {
                    gm.GetComponent<GameManager>().StartCtrl();
                    gm.GetComponent<GameManager>().UseBar();
                }
                if(Book) {
                    Book.GetComponent<BookTut>().FocusOnDelay();
                }
            }
        }
    }

    private void StandShow() {
        player.transform.Find("LiamMesh").gameObject.SetActive(false);
        StarPlatinum.SetActive(true);
    }

    private void StandRetrieve() {
        StarPlatinum.SetActive(false);
        player.transform.Find("LiamMesh").gameObject.SetActive(true);
    }
}
