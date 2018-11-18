using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BridgeTrigger : MonoBehaviour {

    public GameObject player;
    public PlayableDirector timeline;

    private bool hasPlayed = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(hasPlayed == false && Vector3.Distance(player.transform.position, transform.position) < 1.0f) {
            hasPlayed = true;
            timeline.Play();
        }
        //Debug.Log("Trigger distance: " + Vector3.Distance(player.transform.position, transform.position));
    }
}
