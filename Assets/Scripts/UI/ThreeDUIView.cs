using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDUIView : MonoBehaviour {

    [Tooltip("UI字体颜色(大概平时为白色，警告字体为红色)")]
    public Color textColor = Color.white;

    [Tooltip("展开动画播放完毕后面板的最终高度(Scale)")]
    public float targetHeight = 1;

    [Tooltip("面板展开的速度(以帧为单位)")]
    public float panelSpreadSpeed = 0.08f;
    
    [Tooltip("提示文字出现时由不透明变成透明的速度(以帧为单位)")]
    public float textShowUpSpeed = 0.01f;

    [Tooltip("提示文字明暗闪烁透明度变化的速度(以帧为单位)")]
    public float textShineSpeed = 0.006f;

    private GameObject panel;
    private GameObject text;

    /// <summary>
    /// status: Which indicate the status of the 3dUI
    ///         status = 0 --- the 3dUI has not been activated or has been deactivated
    ///         status = 1 --- the 3dUI has been completely activated
    ///         status = 2 --- the 3dUI is being activated
    ///         status = 3 --- the 3dUI is being deactivated
    /// </summary>
    private int status = 0;
    private bool isDecline = true;

	// Use this for initialization
	void Start () {
        Init();

        // For test ONLY
        //Activate();
	}

    private void Init() {
        panel = gameObject.transform.Find("3dUIPanel").gameObject;
        text = gameObject.transform.Find("Text").gameObject;

        // Set text invisible
        Color tmpColor = text.GetComponent<TextMesh>().color;
        tmpColor.a = 0;
        text.GetComponent<TextMesh>().color = tmpColor;

        // Set panel's height to 0
        Vector3 tmpScale = panel.GetComponent<Transform>().localScale;
        tmpScale.y = 0;
        panel.GetComponent<Transform>().localScale = tmpScale;

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // For test ONLY
        if(Input.GetKeyDown(KeyCode.K)) {
            Deactivate();
        }
        if(Input.GetKeyDown(KeyCode.J)) {
            Activate();
        }


        if(status == 2) {// activate
            if(panel.GetComponent<Transform>().localScale.y <= targetHeight - panelSpreadSpeed) {
                Vector3 tmpScale = panel.GetComponent<Transform>().localScale;
                tmpScale.y += panelSpreadSpeed;
                panel.GetComponent<Transform>().localScale = tmpScale;
            }
            else {
                if(text.GetComponent<TextMesh>().color.a <= 1.0f - textShowUpSpeed) {
                    Color tmpColor = text.GetComponent<TextMesh>().color;
                    tmpColor.a += textShowUpSpeed;
                    text.GetComponent<TextMesh>().color = tmpColor;
                }
                else {
                    status = 1;
                    isDecline = true;
                }
            }
        }
        else if(status == 3) {// deactivate
            if(text.GetComponent<TextMesh>().color.a > 0) {
                Color tmpColor = text.GetComponent<TextMesh>().color;
                tmpColor.a = 0;
                text.GetComponent<TextMesh>().color = tmpColor;
            }
            if(panel.GetComponent<Transform>().localScale.y >= 0 + panelSpreadSpeed) {
                Vector3 tmpScale = panel.GetComponent<Transform>().localScale;
                tmpScale.y -= panelSpreadSpeed;
                panel.GetComponent<Transform>().localScale = tmpScale;
            }
            else {
                status = 0;
            }
        }
        else if(status == 1) {// normal shine
            if(isDecline == true) {
                if(text.GetComponent<TextMesh>().color.a >= 0.4f + textShineSpeed) {
                    Color tmpColor = text.GetComponent<TextMesh>().color;
                    tmpColor.a -= textShineSpeed;
                    text.GetComponent<TextMesh>().color = tmpColor;
                }
                else {
                    isDecline = false;
                }
            }
            else {
                if(text.GetComponent<TextMesh>().color.a <= 1.0f - textShineSpeed) {
                    Color tmpColor = text.GetComponent<TextMesh>().color;
                    tmpColor.a += textShineSpeed;
                    text.GetComponent<TextMesh>().color = tmpColor;
                }
                else {
                    isDecline = true;
                }
            }
        }
	}

    public void Activate() {
        status = 2;
    }

    public void Deactivate() {
        status = 3;
    }
}
