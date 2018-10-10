//----------------------------------------------------------------------------------------------
// Simple example script to handle character animation
// Please update it or create new according to requirements of your project
// You also can now randomize the animation in few ways:
//  1) Simple by direct calling functions, RandomizeIdle(), RandomizeMove(), etc
//  2) By adding calling those functions form the waypoint: 
//  3) By using special script for trigger - ChangeAnimation_Trigger. Just attach it to any object with collider and  chose which animation you want to randomize when actor enters the  trigger-zone.
//----------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointMover))]
public class CharacterActions : MonoBehaviour 
{
	public WaypointMover waypointMover;

	public AnimationClip idleAnimation;
	public AnimationClip[] idleVariations;

	public AnimationClip moveAnimation;
	public AnimationClip[] moveVariations;

	public AnimationClip jumpAnimation;
	public AnimationClip[] jumpVariations;

	public Vector3 jumpForce;


	bool inSpecialAction = false;
	Animation animationComponent;


	//----------------------------------------------------------------------------------
	void Start () 
	{
		if (!waypointMover)  
			waypointMover = GetComponent<WaypointMover>();
		
		animationComponent = GetComponent<Animation>();

		if (!idleAnimation  &&  idleVariations.Length > 0)  idleAnimation = idleVariations[0];
		if (!moveAnimation  &&  moveVariations.Length > 0)  moveAnimation = moveVariations[0];
		if (!jumpAnimation  &&  jumpVariations.Length > 0)  jumpAnimation = jumpVariations[0];
	}

	//----------------------------------------------------------------------------------
	void Update () 
	{
		if (!inSpecialAction) 
		{
			if (waypointMover.isMoving ())
				Move ();
			else
				Idle ();
		}
		else
			if (!animationComponent.isPlaying) 
				inSpecialAction = false;
	}

	//----------------------------------------------------------------------------------
	void Idle () 
	{
		if (!animationComponent.isPlaying || animationComponent.IsPlaying(moveAnimation.name)) 
			animationComponent.Play(idleAnimation.name);
	}

	//----------------------------------------------------------------------------------
	void Move () 
	{
		if (animationComponent.isPlaying)  
			animationComponent.Play(moveAnimation.name);
	}

	//----------------------------------------------------------------------------------
	void Jump() 
	{
		inSpecialAction = true;
		animationComponent.Play(jumpAnimation.name);
		GetComponent<Rigidbody>().AddRelativeForce(jumpForce, ForceMode.Impulse);
	}


	//----------------------------------------------------------------------------------
	public AnimationClip RandomizeAnimation(AnimationClip[] variations)
	{
		return variations[Random.Range(0, variations.Length)];
	}

	//----------------------------------------------------------------------------------
	public void RandomizeIdle() 
	{
		if (idleVariations.Length > 0) 
			idleAnimation = RandomizeAnimation (idleVariations);
	}

	//----------------------------------------------------------------------------------
	public void RandomizeMove() 
	{
		if (moveVariations.Length > 0)  
			moveAnimation = RandomizeAnimation (moveVariations);
	}

	//----------------------------------------------------------------------------------
	public void RandomizeJump() 
	{
		if (jumpVariations.Length > 0)  
			jumpAnimation = RandomizeAnimation (jumpVariations);
	}

	//----------------------------------------------------------------------------------
}