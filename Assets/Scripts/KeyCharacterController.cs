using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

// 这个类用于实现玩家对着key按E之后角色自动走到和key重合
// 也用于在key成功激活之后启动timeline

public class KeyCharacterController : MonoBehaviour {

    private GameObject character;
    private ThirdPersonCharacter tppC;

    private bool attracting = false;

	// Use this for initialization
	void Start () {
        character = GameObject.FindWithTag("TppCharacter");
        tppC = character.GetComponent<ThirdPersonCharacter>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(attracting == true) {
            if(Vector3.Distance(character.transform.position, transform.position) > 1.3) {
               
                Vector3 direct = transform.position - character.transform.position;
                direct.y = 0;
                direct = direct.normalized;
                tppC.Move(direct, false, false);
            }
            else {
                attracting = false;
            }
        }
	}

    void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            GotoKey();
        }
    }

    public void GotoKey() {
        attracting = true;
    }
}
