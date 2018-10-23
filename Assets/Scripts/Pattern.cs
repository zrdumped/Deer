using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour {
    public enum PitchType { Ultralow, Low, Medium, High, Ultrahigh};
    [Header("Pattern Control")]
    public float minScale = 1;
    public float maxScale = 10;
    public int pitchLength = 100;  //seconds
    public int intermissionLength = 2;   //multiple of pitch length
    public int changeFrames = 20; 
    public PitchType[] pattern;
    public bool onShow = false;
    public Key keyController;
    [Header("Debug")]
    public bool matchResult = true;


    private int pitchID = 0;
    private int patternLength;
    private float[] patternScale = new float[5];
    private float scaleChangeStep = 0;
    private int changedFrame = 0;

	// Use this for initialization
	void Start () {
        patternLength = pattern.Length;
        float step = (maxScale - minScale) / 4.0f;
        patternScale[0] = minScale;
        patternScale[1] = minScale + step * 1;
        patternScale[2] = minScale + step * 2;
        patternScale[3] = minScale + step * 3;
        patternScale[4] = maxScale;
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    public string StartHint()
    {
        StartCoroutine("showHint");
        onShow = true;
        //Debug.Log("Start");
        return "Press 'E' to Stop";
    }

    public string StopHint()
    {
        StopCoroutine("showHint");
        onShow = false;
        this.transform.localScale = new Vector3(0, 0, 0);
        pitchID = 0;
        scaleChangeStep = 0;
        changedFrame = 0;
        //Debug.Log("Stop");
        return "Press 'E' to Display";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            matchResult = !matchResult;
    }

    void FixedUpdate()
    {
        if (scaleChangeStep != 0 && onShow)
        {
            this.transform.localScale += new Vector3(scaleChangeStep, scaleChangeStep, scaleChangeStep);
            changedFrame++;
            if (changedFrame == changeFrames)
            {
                scaleChangeStep = 0;
                changedFrame = 0;
            }
        }
    }

	public int GetCurrentPitchId() {
		return pitchID;
	}

    // Update is called once per frame
    IEnumerator showHint() {
        while (true)
        {
            if(pitchID > 0 || pitchID == -intermissionLength)
            {
                //get match result, replaced by matchResult temporarily
                int waitSeconds;
                if (matchResult)
                    waitSeconds = keyController.MatchSucceed();
                else
                {
                    waitSeconds = keyController.MatchFail();
                    pitchID = -1;
                }
                yield return new WaitForSeconds(waitSeconds);
            }

            if(pitchID == -intermissionLength)
            {
                keyController.activate();
            }

            float targetScale;
            if (pitchID >= 0)
            {
                targetScale = patternScale[(int)pattern[pitchID]];
                //this.transform.localScale = new Vector3(curScale, curScale, curScale);
            }
            else
            {
                targetScale = 0;
                //this.transform.localScale = new Vector3(0, 0, 0);
            }
            //SetTargetScale(targetScale);
            scaleChangeStep = (targetScale - this.transform.localScale.x) / (float)changeFrames;
            //Debug.Log(scaleChangeStep);
            pitchID++;
            if (pitchID == pattern.Length)
                pitchID -= intermissionLength;
            yield return new WaitForSeconds(pitchLength);
        }
    }
}
