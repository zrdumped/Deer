using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class BookTut : MonoBehaviour {

    // How high will the book float above the original position
    public float FloatingHeight = 0.3f;
    public Material normalMat;
    public Material highlightMat;
    public GameObject character;
    public GameObject TreeDUI;

    private GameManager gm;
    private Vector3 originePos;
    private bool bookTouched = false;
    private bool lightBallTouchedBook = false;
    private bool focusOn = false;

	// Use this for initialization
	void Start () {
        originePos = transform.position;
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        //if(Input.GetKeyDown(KeyCode.G)) {
        //    FocusOn();
        //}
        //if(Input.GetKeyDown(KeyCode.B)) {
        //    OnBookTouched();
        //}

        if(bookTouched == true && Input.GetKeyDown(KeyCode.E)) {
            OnEPressed();
        }

        if(lightBallTouchedBook == false && focusOn == true) {
            Vector3 lightballPos = character.transform.Find("LightBall").transform.position;

            if(Vector3.Distance(lightballPos, transform.position)
                    < character.GetComponent<Character>().GetLightBallRadius()) {
                lightBallTouchedBook = true;
                OnBookTouched();
            }
        }
	}


    public void FocusOnDelay() {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.5f);
        sequence.AppendCallback(FocusOn);
    }

    public void FocusOn() {
        if(gm && character) {
            gm.GetComponent<GameManager>().StopCtrl();
            Vector3 pos = transform.position;
            pos.y = character.transform.position.y;

            gm.interactMgr.SetTextUntimed("Use your voice with different pitches through microphone to control the light. Try to touch the book with the light surrounding you.");

            Sequence sequence = DOTween.Sequence();
            sequence.Append(character.transform.DOLookAt(pos, 1.0f));
            sequence.AppendCallback(gm.GetComponent<GameManager>().StartCtrl);
            sequence.AppendCallback(gm.GetComponent<GameManager>().StopMoveCtrl);
            sequence.AppendCallback(FloatInAir);

            focusOn = true;

        }
        else {
            Debug.Log("Couldn't find gm or character.");
        }
    }

    private void FloatInAir() {
        Vector3 targetPos = transform.position + Vector3.up * FloatingHeight;
        targetPos.z += 0.8f;
        transform.DOMove(targetPos, 1.5f);
        if(highlightMat) {
            gameObject.GetComponent<MeshRenderer>().material = highlightMat;
        }
    }

    private void ReturnToOrigin() {
        transform.DOMove(originePos, 1.5f);
        if(normalMat) {
            gameObject.GetComponent<MeshRenderer>().material = normalMat;
        }
    }

    private void OnBookTouched() {
        ReturnToOrigin();
        gm.interactMgr.SetTextUntimed("Press E to get message from the book.");
        bookTouched = true;
    }

    private void OnEPressed() {
        bookTouched = false;
        gm.GetComponent<GameManager>().StartMoveCtrl();
        gm.interactMgr.ClearText();
        // Show 3dUI
        if(TreeDUI) {
            TreeDUI.GetComponent<ThreeDUIView>().Activate();
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(originePos, 15.0f));
        // Close 3dUI
        if(TreeDUI) {
            sequence.AppendCallback(TreeDUI.GetComponent<ThreeDUIView>().Deactivate);
        }
    }
}
