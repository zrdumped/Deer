using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObjManager : MonoBehaviour {

    public Text hintText;
    public float highLightThreshod = 5;
    public float hintThreshod = 2;

    private InteractiveObjList interactiveObjList;
    private int status = 0; // 0:Do Nothing; 1:Loop in the List per frame
    private GameObject character;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(status == 1 && character) {
            foreach(var obj in interactiveObjList.list) {

                InteractiveObj tmp = obj.GetComponent<InteractiveObj>();

                // Set material to normal or highlight according to distance.
                if(Vector3.Distance(character.transform.position, obj.transform.position) < highLightThreshod) {
                    if(tmp) {
                        tmp.SetHighlight(true);
                    }
                }
                else {
                    if(tmp) {
                        tmp.SetHighlight(false);
                    }
                }

                // Show or hide the hint msg according to distance and toward direct.
                Vector3 direct = obj.transform.position - character.transform.position;
                if(Vector3.Distance(character.transform.position, obj.transform.position) < hintThreshod
                    && Vector3.Dot(direct.normalized, character.transform.forward) > 0.5) {
                    hintText.text = tmp.GetTextContent(0);
                }
                else {
                    
                }
            }
        }
	}

    public void StopLooping() {
        status = 0;
    }

    public void SetIntObjList(InteractiveObjList intObjList) {
        interactiveObjList = intObjList;
        status = 1;
    }

    public void SetCharacter(GameObject characterIn) {
        character = characterIn;
    }


}
