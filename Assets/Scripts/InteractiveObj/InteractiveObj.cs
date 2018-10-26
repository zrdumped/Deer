using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObj : MonoBehaviour {

    public List<string> textContents;
    public Material highlightMat;
    public Material normalMat;

    // flag to mark whether this object is within the interactive range.
    // 1 means near enough to interact, 0 means not.
    private int status = 0;

	// Use this for initialization
	void Start () {
		// TODO: Find UICanvas in BaseScene 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Check and change the obj's material(NEED to deduplicate)
    // isHighlight == true --- use highlight mat; isHighlight == false --- use normal mat.
    public void SetHighlight(bool isHighlight) {
        if(isHighlight == true) {
            if(highlightMat) {
                gameObject.GetComponent<MeshRenderer>().material = highlightMat;
            }
        }
        else {
            if(normalMat) {
                gameObject.GetComponent<MeshRenderer>().material = normalMat;
            }
        }
    }

    // InteractiveObjManager will call this function to get text content.
    // idx: one object might have multiple text to store.
    public string GetTextContent(int idx) {
        return textContents[idx];
    }

    // Set current gameObject to be interactive, 
    public void SetInteractive(bool isInteracive) {
        if(isInteracive) {
            status = 1;
        }
        else {
            status = 0;
        }
    }

    // TODO: Deal with 3dUI system
}
