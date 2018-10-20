//----------------------------------------------------------------------------------
// Allows to randomize the animation of Waypoint mover
// Just attach it to any object with collider and  chose which animation you want to randomize when actor enters the  trigger-zone.
//----------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeAnimation_Trigger : MonoBehaviour 
{
	public enum AnimationToChange {Idle, Move, Jump}


	public AnimationToChange animationToChange;

	//----------------------------------------------------------------------------------
	void Start () 
	{
		if (!gameObject.GetComponent<Collider>().isTrigger) gameObject.GetComponent<Collider>().isTrigger = true;

	}

	//----------------------------------------------------------------------------------
	void  OnTriggerEnter (Collider other) 
	{
		CharacterActions characterActions = other.GetComponent<CharacterActions>(); 

		if (characterActions)
			switch (animationToChange)
			{
				case AnimationToChange.Idle :
					characterActions.RandomizeIdle();
					break;

				case AnimationToChange.Move :
					characterActions.RandomizeMove();
					break;

				case AnimationToChange.Jump :
					characterActions.RandomizeJump();
					break;
			}

	}
	//----------------------------------------------------------------------------------
}
