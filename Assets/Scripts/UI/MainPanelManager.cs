using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class MainPanelManager : MonoBehaviour {

    public Transform mainPanel;
    public Transform escapePanel;
    public Transform mainPanelTarget;
    public Transform mainPanelHome;
    public Transform escPanelTarget;
    public Transform escPanelHome;
    public GameObject canvasLogo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.T)) {
            ShowMainPanel();
        }
        if(Input.GetKeyDown(KeyCode.U)) {
            HideMainPanel();
        }
    }

    public void ShowMainPanel() {
        canvasLogo.SetActive(true);
        mainPanel.gameObject.SetActive(true);
        mainPanel.DOMove(mainPanelTarget.position, 0.3f);
    }

    public void HideMainPanel() {
        //mainPanel.DOMove(mainPanelHome.position, 0.3f);
        //Debug.Log("MainPanel Move to Home Position");
        mainPanel.gameObject.SetActive(false);
        canvasLogo.SetActive(false);
    }

    //public void ShowStartPanel() {
    //    HideMainPanel();
    //    startPanel.DOMove(startPanelTarget.position, 0.3f);
    //}

    //public void HideStartPanel() {
    //    ShowMainPanel();
    //    startPanel.DOMove(startPanelHome.position, 0.3f);
    //}

    public void ShowEscPanel() {
        escapePanel.DOMove(escPanelTarget.position, 0.3f);
    }

    public void HideEscPanel() {
        escapePanel.DOMove(escPanelHome.position, 0.3f);
    }

    public void Reset() {
        Debug.Log("Reset MainPanelManager");
        HideMainPanel();
    }
}
