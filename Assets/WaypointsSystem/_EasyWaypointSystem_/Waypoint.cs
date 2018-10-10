//----------------------------------------------------------------------------------------------
// Basic script that contains waypoint visualisation options and associated actions
//----------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Waypoint : MonoBehaviour 
{
	// Debug visualization options
	public Color color;				// Waypoint gizmo color
	public float radius = 0.25f;   	// Waypoint gizmo size
	public string iconName; 		// Waypoint gizmo icon filename 

	// Associated actions
	public float delay = 0;			// Delay movement for next waypoint, when Mover object rich the waypoint
	public float angle = 0;
	public string callFunction;		// Call function with this name, when Mover object rich the waypoint
	public string callExitFunction;	// Call function with this name, when Mover object leaves the waypoint
	public float newMoverSpeed;  	// If more than 0 - will change current WaipointMover speed to this


	//=============================================================================================================
	// Draw debug visualization
	void OnDrawGizmos () 
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, radius);

		if (iconName !="") 
			Gizmos.DrawIcon (new Vector3(transform.position.x, transform.position.y+radius*1.5f, transform.position.z), iconName, true);

	}

	//----------------------------------------------------------------------------------
}
