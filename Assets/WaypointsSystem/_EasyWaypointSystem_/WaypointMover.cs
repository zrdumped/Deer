//----------------------------------------------------------------------------------
// The most important script. 
// It manages waypointed path from waypointsHolder and move object along it.
// Script also allows to setup different following and looping types for  movement.
//----------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaypointMover : MonoBehaviour 
{
	// List of following types
	public enum FollowType 
	{ 
		Simple,		  // Just move object as it is (without any rotation or dumping)
		Facing,		  // Roughly face object on current waypoint
		SmoothFacing,  // Face object on current waypoint and adapt path smoothly 
		SmoothDamping, // Speed will be decreased before each waypoint and accelerate after it 
		Simple2D,      // Simple movementt in 2D
		Facing2D,      // Roughly face object on current waypoint in 2D
		SmoothFacing2D,// Smoothly face object on current waypoint in 2D
		Teleport,		  // Simply teleport (transfer immediately) object to current waypoin position
		InvertedFacing
	}

	// List of looping types
	public enum LoopType 
	{ 
		Once,		 // Only one cycle
		Cycled,		 // Infinite amounts of cycles
		PingPong,	 // Move object in another direction when it gets first/last point of path
		SeveralTimes  // Repeat loop several times (specified in numberOfLoops)
	}

	// List of axes waypoint position along which should be ignored
	[System.Serializable]
	public struct UsedAxis
	{
		public bool x;
		public bool y;
		public bool z;
	}



	public WaypointsHolder waypointsHolder;			// Move along the path holded in this WaypointsHolder
	public FollowType followingType;					// Choose one of following type to use
	public LoopType loopingType;						// Choose one of looping type to use
	public bool MoveOnWayImmediately = false;		// Move object immediately to the first waypoint at start
	public bool StartFromNearestWaypoint = false;	// Start movement from the nearest waupoint
	public UsedAxis ignorePositionAtAxis;				// Ignore waypoint position along those axis
	public float damping = 3.0f;								// Smooth facing/movement value
	public float movementSpeed = 5.0f;					// Speed of object movement along the path
	public float waypointActivationDistance = 1.0f;	// How far should object be to waypoint for its activation and choosing new
	public int numberOfLoops = 0;						// How much loops should be performed before stop. Use this parameter if loopingType=SeveralTimes
	public float preventCollisionDistance;            // Object suspend movement if there is any obstacle on this distance (in front of him)
	public bool smoothCollisionPreventing = false;	// Mover will be smoothly decrease speed in front of obstacle
	public bool dynamicWaypointsUpdate = false;    // Respect Waypoints position update dynamically


	// Usefull internal variables, please don't change them blindly
	int currentWaypoint = 0;
	int direction = 1;
	Vector3 velocity = Vector3.zero;
	Vector3 targetPosition;
	float delayTillTime;
	int loopNumber = 1;
	bool inMove = false;
	bool suspended = false;
	int previousWaypoint = 0;
	float initialMovementSpeed;
	bool callExitFunction = false;
	bool onWaypoint;


    private void OnEnable()
    {
        //currentWaypoint = 0;
        //direction = 1;
        //velocity = Vector3.zero;
        //loopNumber = 1;
        //inMove = false;
        //suspended = false;
        previousWaypoint = 0;
        //callExitFunction = false;
        Start();
    }

    //=============================================================================================================
    // Setup initial data according to specified parameters
    void Start () 
	{
		if (waypointsHolder == null) 
		{
			Debug.LogWarning ("No WaypointsHolder attached to " + gameObject.name);
			return;
		}

		// Make the rigidbody not change rotation
		if (GetComponent<Rigidbody>()) 
			GetComponent<Rigidbody>().freezeRotation = true;


		initialMovementSpeed = movementSpeed;

		if (StartFromNearestWaypoint)
		{
			Vector3 waypointPosition;
			int nearestWaypointID = 0;
			float previousSmallestDistance = Mathf.Infinity;
			float distance;

			for (int i = 0; i < waypointsHolder.waypoints.Count; i++)
			{
				waypointPosition = waypointsHolder.waypoints[i].gameObject.transform.position;
				waypointPosition = IgnorePositionByAxis(waypointPosition);

				distance = Vector3.Distance(transform.position, waypointPosition);
				if (distance < previousSmallestDistance)
				{
					nearestWaypointID = i;
					previousSmallestDistance = distance;
				}

			}

			currentWaypoint = nearestWaypointID;
		}
		else 
			currentWaypoint = 0;


		if(MoveOnWayImmediately) 
			transform.position = waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position;


		targetPosition = waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position;
		targetPosition = IgnorePositionByAxis(targetPosition);

	}

	//----------------------------------------------------------------------------------
	//Main loop
	void Update () 
	{
		if (waypointsHolder == null)  return;

		bool collisionPrevented = false;
		RaycastHit hit;
		Vector3 p1 = transform.position;
		Vector3 p2 = p1;


		// Cast character controller shape preventCollisionDistance meters forward, to see if it is about to hit anything
		if (preventCollisionDistance > 0)
			if (Physics.CapsuleCast (p1, p2, 0.5f, transform.forward, out hit, preventCollisionDistance)) 
			{
				if (!smoothCollisionPreventing) 
					collisionPrevented = true;
				else // smoothly decrease speed infront of obstacle
				{
					movementSpeed = initialMovementSpeed * Vector3.Distance(transform.position, hit.point) / preventCollisionDistance;
					if (movementSpeed < initialMovementSpeed/preventCollisionDistance) 
						collisionPrevented = true;
				}
			}
			else
				movementSpeed = initialMovementSpeed; 



		// Respect current Waypoint position-update dynamicaly (even if mover is already on the way to this waypoint)
		if (dynamicWaypointsUpdate  &&  currentWaypoint >= 0) 
			if(targetPosition != waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position)
			{       
				targetPosition = waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position;
				transform.LookAt(targetPosition);
				targetPosition = IgnorePositionByAxis(targetPosition);
			}  



		// Process movement if waypoint exists and there is no delay assigned to it
		if (!suspended  &&  !collisionPrevented  &&  currentWaypoint >= 0  &&  delayTillTime < Time.time) 
		{
			inMove = true; 

			// Activate waypoint when object is closer than waypointActivationDistance
			if(Vector3.Distance(transform.position, targetPosition) < waypointActivationDistance) 
			{
				// Init delay if it's specified in waypoint
				if (waypointsHolder.waypoints[currentWaypoint].delay > 0) 
					delayTillTime = Time.time + waypointsHolder.waypoints[currentWaypoint].delay;
				
				// Try to call function in object if there is any function name specified in waypoint "callFunction" parameter
				if (waypointsHolder.waypoints[currentWaypoint].callFunction != "") 
					SendMessage (waypointsHolder.waypoints[currentWaypoint].callFunction, SendMessageOptions.DontRequireReceiver);

				// If waypoint have specified newMoverSpeed bigger than 0 -  change current WaipointMover speed 
				if(waypointsHolder.waypoints[currentWaypoint].newMoverSpeed > 0) 
					ChangeWaypointMoverSpeed(waypointsHolder.waypoints[currentWaypoint].newMoverSpeed);


				// Select next waypoint according to direction 
				previousWaypoint = currentWaypoint;
				currentWaypoint += direction;
				onWaypoint = true;
				// currentWaypoint = Random.Range(0, waypointsHolder.waypoints.Length);


				// Choose next waypoint/actions according to loopingType, if object reaches first or last  waypoint
				if(currentWaypoint > waypointsHolder.waypoints.Count-1 || currentWaypoint<0)
					switch (loopingType)
					{
						case LoopType.Once: 
							currentWaypoint = -1;
							break;

						case LoopType.Cycled:
							currentWaypoint = currentWaypoint < 0 ? waypointsHolder.waypoints.Count-1 : 0; 
							break;

						case LoopType.PingPong:
							direction = -direction;
							currentWaypoint += direction;
							break;

						case LoopType.SeveralTimes:
							if (loopNumber < numberOfLoops) 
							{
								currentWaypoint = 0;
								loopNumber++;
							}
							else
								currentWaypoint = -1;
							break;
					}


				// Get/update next waypoint XYZ position in World coordinates
				if(currentWaypoint >= 0  &&  waypointsHolder.waypoints[currentWaypoint])
				{  	 
					targetPosition = waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position;
					targetPosition = IgnorePositionByAxis(targetPosition); 
				}
				else
					if (currentWaypoint < waypointsHolder.waypoints.Count  &&  currentWaypoint >= 0)
					{ 
						currentWaypoint -= direction;
						Debug.LogWarning("Waypoint is missed in " + waypointsHolder.gameObject.name);
					}

				callExitFunction = true;	      

			}
			else  // When object leaves waypoint - try to call function(specified in waypoint "callExitFunction" parameter) in object
			{
				onWaypoint = false;

				if(waypointsHolder.waypoints[previousWaypoint].callExitFunction != ""  &&  callExitFunction)
					if(Vector3.Distance(transform.position, waypointsHolder.waypoints[previousWaypoint].gameObject.transform.position) < waypointActivationDistance) 
					{
						SendMessage (waypointsHolder.waypoints[previousWaypoint].callExitFunction, SendMessageOptions.DontRequireReceiver); 
						callExitFunction = false;
					}
			}


	// Choose or update rotation/facing according to facingType

			// Look at and dampen the rotation
			if (followingType == FollowType.SmoothFacing)
			{
				Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
				transform.Translate(Vector3.forward*movementSpeed*Time.deltaTime);
			}

			// Just Look at	
			if (followingType == FollowType.Facing)
			{
				transform.LookAt(targetPosition);
				transform.Translate(Vector3.forward*movementSpeed*Time.deltaTime);
			}

			// Inverted Look at	
			if (followingType == FollowType.InvertedFacing)
			{
				transform.LookAt(targetPosition);
				transform.Rotate(Vector3.up, 180);
				transform.Translate(-Vector3.forward*movementSpeed*Time.deltaTime);
			}



			// Move without rotation
			if (followingType == FollowType.Simple)  
				transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);   


			if (followingType == FollowType.SmoothDamping)
				transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, movementSpeed);


			if (followingType == FollowType.Simple2D)
			{
				SmoothLookAt2D(transform, targetPosition, damping);
				transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
			}	


			// Roughly face object on current waypoint in 2D
			if (followingType == FollowType.Facing2D)
			{
				Vector3 targetDir = targetPosition - transform.position;
				float angle = Vector3.Angle(targetDir, transform.right);

				if(angle > 3) 
					transform.eulerAngles = new Vector3 (
															transform.rotation.eulerAngles.x,
															transform.rotation.eulerAngles.y,
															transform.rotation.eulerAngles.z + angle
														);

				transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
			}


			// Smoothly face object on current waypoint in 2D
			if (followingType == FollowType.SmoothFacing2D)
			{
				Vector3 targetDir = targetPosition - transform.position;
				float angle = Vector3.Angle(targetDir, transform.right);

				if(angle > 3) 
					transform.eulerAngles = new Vector3
							(
								transform.rotation.eulerAngles.x,
								transform.rotation.eulerAngles.y,
								Mathf.Lerp(transform.rotation.eulerAngles.z, transform.rotation.eulerAngles.z - angle, damping * Time.deltaTime)
							);					

				transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
			} 


			if (followingType == FollowType.Teleport) 
				transform.position = targetPosition;


			float value = Vector3.Distance(waypointsHolder.waypoints[previousWaypoint].gameObject.transform.position, waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position);  
			value = Vector3.Distance(transform.position, targetPosition) / value;  

			transform.localEulerAngles = new Vector3 (
						transform.localEulerAngles.x,
						transform.localEulerAngles.y,
						Mathf.Lerp(waypointsHolder.waypoints[previousWaypoint].angle, waypointsHolder.waypoints[currentWaypoint].angle, 1.0f - value)
					);			 

		}
		else 
			inMove = false;  
		
	}

	//----------------------------------------------------------------------------------
	// Reverse path-movement direction and sends object to previous waypoint
	public void ReverseDirection () 
	{
		previousWaypoint = currentWaypoint;
		direction = -direction;

		currentWaypoint =  direction > 0 ? currentWaypoint+1 : currentWaypoint-1;

		if (currentWaypoint < 0) 
			currentWaypoint =  waypointsHolder.waypoints.Count-1; 
		else 
			if (currentWaypoint > waypointsHolder.waypoints.Count-1) 
				currentWaypoint = 0; 

		targetPosition = waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position;
	}

	//----------------------------------------------------------------------------------
	// Set path-movement direction and sends object to related waypoint
	public void SetDirection (int _direction) 
	{
		previousWaypoint = currentWaypoint;
		currentWaypoint =  _direction > 0 ? currentWaypoint+1 : currentWaypoint-1; 

		if (currentWaypoint < 0) 
			currentWaypoint =  waypointsHolder.waypoints.Count-1; 
		else 
			if (currentWaypoint > waypointsHolder.waypoints.Count-1) 
				currentWaypoint = 0; 

		targetPosition = waypointsHolder.waypoints[currentWaypoint].gameObject.transform.position;
	}

	//----------------------------------------------------------------------------------
	// Return is mover on Waypoint  or not
	public bool IsOnWaypoint ()
	{
		return onWaypoint;
	}

	//----------------------------------------------------------------------------------
	// Return object to position of previous waypoint
	public void ReturnToPreviousWaypoint () 
	{
		currentWaypoint = previousWaypoint;
		transform.position = waypointsHolder.waypoints[previousWaypoint].gameObject.transform.position;
		targetPosition = waypointsHolder.waypoints[previousWaypoint].gameObject.transform.position;
	}

	//----------------------------------------------------------------------------------
	// Return true if object is moving now
	public bool isMoving () 
	{
		return inMove;
	}

	//----------------------------------------------------------------------------------
	// Fully suspend waypoint-controlled movement
	public void Suspend (bool state) 
	{
		suspended = state;
	}

	//----------------------------------------------------------------------------------
	// Fully suspend waypoint-controlled movement
	public void Pause () 
	{
		suspended = true;
	}

	//----------------------------------------------------------------------------------
	// Resumes suspendede movement
	public void Unpause () 
	{
		suspended = false;
	}

	//----------------------------------------------------------------------------------
	// Set new movement speed 
	public void ChangeWaypointMoverSpeed (float newSpeed) 
	{
		initialMovementSpeed = newSpeed;
	}

	//----------------------------------------------------------------------------------
	// Reset position along ignored axis to transform.position
	public Vector3 IgnorePositionByAxis(Vector3 positionToUpdate)
	{
		Vector3 updatedPos = positionToUpdate;

		if (ignorePositionAtAxis.x)  updatedPos.x = transform.position.x;
		if (ignorePositionAtAxis.y)  updatedPos.y = transform.position.y;
		if (ignorePositionAtAxis.z)  updatedPos.z = transform.position.z;

		return updatedPos;
	}


	//----------------------------------------------------------------------------------
	// Smoothly LookAt targetPosition in 2D
	public void SmoothLookAt2D (Transform objectTransform, Vector2 targetPosition, float smoothingValue) 
	{
		Vector3 relative = objectTransform.InverseTransformPoint(targetPosition);
		float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;

		objectTransform.Rotate (0, 0, Mathf.LerpAngle(0, angle, Time.deltaTime * smoothingValue) );
	}

	//----------------------------------------------------------------------------------

}