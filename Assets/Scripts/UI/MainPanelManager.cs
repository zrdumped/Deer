using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class MainPanelManager : MonoBehaviour {

    public Transform mainPanel;
    public Transform startPanel;
    public Transform mainPanelTarget;
    public Transform startPanelTarget;
    public Transform mainPanelHome;
    public Transform startPanelHome;

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
        mainPanel.DOMove(mainPanelTarget.position, 0.3f);
    }

    public void HideMainPanel() {
        mainPanel.DOMove(mainPanelHome.position, 0.3f);
    }

    public void ShowStartPanel() {
        HideMainPanel();
        startPanel.DOMove(startPanelTarget.position, 0.3f);
    }

    public void HideStartPanel() {
        ShowMainPanel();
        startPanel.DOMove(startPanelHome.position, 0.3f);
    }

    public void Reset() {
        HideMainPanel();
        HideStartPanel();
    }
}
