using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {


    public enum PitchType { Ultralow, Low, Medium, High, Ultrahigh};
    [Header("Pattern Control")]
    public int minScale = 1;
    public int maxScale = 10;
    public int pitchLength = 100;  //seconds
    public int intermissionLength = 2;   //multiple of pitch length
    public PitchType[] pattern;


    private int pitchID = 0;
    private int patternLength;
    private float[] patternScale = new float[5];

	// Use this for initialization
	void Start () {
        patternLength = pattern.Length;
        float step = ((float)maxScale - (float)minScale) / 4.0f;
        patternScale[0] = (float)minScale;
        patternScale[1] = (float)minScale + step * 1;
        patternScale[2] = (float)minScale + step * 2;
        patternScale[3] = (float)minScale + step * 3;
        patternScale[4] = (float)maxScale;
    }
	
	// Update is called once per frame
	void showHint() {
        if (pitchID < 0)
        {
            pitchID++;
        }
        else
        {
            float curScale = patternScale[(int)pattern[pitchID]];
            this.transform.localScale = new Vector3(curScale, curScale, curScale);
        }
	}
}
