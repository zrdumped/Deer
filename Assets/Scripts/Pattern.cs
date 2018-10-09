using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour {


    public enum PitchType { Ultralow, Low, Medium, High, Ultrahigh};
    [Header("Pattern Control")]
    public int minScale = 1;
    public int maxScale = 10;
    public int pitchLength = 100;  //seconds
    public int intermissionLength = 2;   //multiple of pitch length
    public int changeFrames = 20; 
    public PitchType[] pattern;


    private int pitchID = 0;
    private int patternLength;
    private float[] patternScale = new float[5];
    public bool onShow = false;
    private float scaleChangeStep = 0;
    private int changedFrame = 0;

	// Use this for initialization
	void Start () {
        patternLength = pattern.Length;
        float step = ((float)maxScale - (float)minScale) / 4.0f;
        patternScale[0] = (float)minScale;
        patternScale[1] = (float)minScale + step * 1;
        patternScale[2] = (float)minScale + step * 2;
        patternScale[3] = (float)minScale + step * 3;
        patternScale[4] = (float)maxScale;
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    public void StartHint()
    {
        StartCoroutine("showHint");
        onShow = true;
        Debug.Log("Start");
    }

    public void StopHint()
    {
        StopCoroutine("showHint");
        onShow = false;
        this.transform.localScale = new Vector3(0, 0, 0);
        pitchID = 0;
        scaleChangeStep = 0;
        changedFrame = 0;
        Debug.Log("Stop");
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

    // Update is called once per frame
    IEnumerator showHint() {
        while (true)
        {
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
            Debug.Log(scaleChangeStep);
            pitchID++;
            if (pitchID == pattern.Length)
                pitchID = -intermissionLength;
            yield return new WaitForSeconds(pitchLength);
        }
    }
}
