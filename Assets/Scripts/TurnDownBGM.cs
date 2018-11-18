using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDownBGM : MonoBehaviour {

    [Range(0,1.0f)]
    public float initVolume = 0.1f;
    [Range(0, 1.0f)]
    public float curVolume = 0.1f;
    public int jumpToChap = 1;
    public int jumpToScene = 1;
    public bool startJump = false;
    private float lastFrameVolume = 0.1f;
    private GameManager gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        curVolume = initVolume;
        lastFrameVolume = initVolume;
    }
	
	// Update is called once per frame
	void Update () {
        if(Mathf.Abs(curVolume - lastFrameVolume) >= 0.001f) {
            gm.audioBGM.volume = curVolume;
            lastFrameVolume = curVolume;
        }

        if(startJump == true) {
            gm.Switch2Scene(jumpToChap, jumpToScene);
            gm.audioBGM.volume = 0.15f;
        }
	}

    public void ChangeVolume(float volume) {
        curVolume = volume;
    }
}
