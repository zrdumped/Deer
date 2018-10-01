using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    public GameObject TargetScrollBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddText(string text)
    {
        this.GetComponent<Text>().text += text + "\n";
        TargetScrollBar.GetComponent<Scrollbar>().value = 0;
    }
}
