using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObj : MonoBehaviour {

    public List<string> textContents;
    public Material highlightMat;
    public Material normalMat;
    public bool need3dUI = false;
    public bool needTextUI = true;
    public GameObject threeDUI;
    public GameObject highlightPart;
    // The object who will listen the instruction and do something.(usually with Key or Door scripts on it)
    public GameObject listenerObj; 



    // flag to mark whether this object is within the interactive range.
    // 1 means near enough to interact, 0 means not.
    public int status = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Check and change the obj's material(NEED to deduplicate)
    // isHighlight == true --- use highlight mat; isHighlight == false --- use normal mat.
    public void SetHighlight(bool isHighlight) {
        if(isHighlight == true) {
            if(highlightMat && highlightPart) {
                highlightPart.GetComponent<MeshRenderer>().material = highlightMat;
            }
        }
        else {
            if(normalMat && highlightPart) {
                highlightPart.GetComponent<MeshRenderer>().material = normalMat;
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
            if(threeDUI && need3dUI == true) {
                threeDUI.GetComponent<ThreeDUIView>().Deactivate();
            }
        }
    }

    // Obsolete
    public void OnInteract() {
        if(threeDUI && need3dUI == true) {
            threeDUI.GetComponent<ThreeDUIView>().Activate();
        }
    }

    // Obsolete
    public void AfterInteract() {
        if(threeDUI && need3dUI == true) {
            threeDUI.GetComponent<ThreeDUIView>().Deactivate();
        }
    }

    public void Trigger() {

    }

    public bool isIntractive() {
        if(status == 1) {
            return true;
        }
        else {
            return false;
        }
    }
}
