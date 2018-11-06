using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveObjManager : MonoBehaviour {

    public Text hintText;
    public Text triggerText;
    public float highLightThreshod = 3;
    public float hintThreshod = 1.5f;
    public KeyCode interactKey = KeyCode.E;

    private InteractiveObjList interactiveObjList;
    private int status = 0; // 0:Do Nothing; 1:Loop in the List per frame
    private GameObject character;
    private Vector3 lightballPos = Vector3.zero;

    // params need for counting down
    private bool startCount = false;
    private float count = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(status == 1 && character) {

            int haveSomethingToShow = 0;

            lightballPos = character.transform.Find("LightBall").transform.position;

            foreach(var obj in interactiveObjList.list) {

                InteractiveObj tmp = obj.GetComponent<InteractiveObj>();

                // objects will be interactive ONLY after they are touched by lightball
                if(Vector3.Distance(lightballPos, tmp.transform.position)
                    < character.GetComponent<Character>().GetLightBallRadius()) {
                    Debug.Log("Interactive Object " + tmp.name + " has been detected.");
                    tmp.hasDetected = true;
                }


                if(tmp.hasDetected == true) {

                    // Set material to normal or highlight according to distance.
                    if(Vector3.Distance(character.transform.position, obj.transform.position) < highLightThreshod) {
                        if(tmp) {
                            //Debug.Log(obj.name + "Set Highlignt Mat");
                            tmp.SetHighlight(true);
                        }
                    }
                    else {
                        if(tmp) {
                            //Debug.Log(obj.name + "Set Normal Mat");
                            tmp.SetHighlight(false);
                        }
                    }

                    // Show or hide the hint msg according to distance and toward direct.
                    Vector3 direct = obj.transform.position - character.transform.position;
                    if(Vector3.Distance(character.transform.position, obj.transform.position) < hintThreshod
                        && Vector3.Dot(direct.normalized, character.transform.forward) > 0.25) {
                        if(tmp.needTextUI == true) {
                            hintText.text = tmp.GetTextContent(0);
                        }
                        haveSomethingToShow++;
                        if(tmp.need3dUI == true) {
                            tmp.SetInteractive(1);
                        }
                        else {
                            tmp.SetInteractive(2);
                        }
                    }
                    else {
                        tmp.SetInteractive(0);
                    }
                }
                
            }

            if(haveSomethingToShow == 0) {
                hintText.text = "";
            }
        }

        if(Input.GetKeyDown(interactKey) && status == 1) {
            foreach(var obj in interactiveObjList.list) {
                InteractiveObj tmp1 = obj.GetComponent<InteractiveObj>();
                if(tmp1.status == 1) {
                    tmp1.threeDUI.GetComponent<ThreeDUIView>().Activate();
                    if(tmp1.needOS == true) {
                        SetTextTimed(tmp1.GetOS(), 5.0f);
                    }
                }
                else if(tmp1.status == 2) {
                    string msgOnTrigger = tmp1.Trigger();
                    if(msgOnTrigger != "") {
                        SetTextTimed(msgOnTrigger, 4.0f);
                    }
                    break;
                }
            }
        }

        if(startCount == true) {
            if(count > Time.deltaTime) {
                count -= Time.deltaTime;
            }
            else {
                count = 0;
                startCount = false;
                triggerText.text = "";
                hintText.enabled = true;
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

    // Set 'text' into hintMsg and it will automatically disappear after 'countDown' seconds.
    public void SetTextTimed(string text, float countDown) {
        hintText.enabled = false;
        triggerText.text = text;
        if (countDown > 0)
        {
            count = countDown;
            startCount = true;
        }
    }


}
