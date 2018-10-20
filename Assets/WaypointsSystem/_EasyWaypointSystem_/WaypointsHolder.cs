//----------------------------------------------------------------------------------------------
// Object with this script hold waypoints as path and visualize it
// If list of waypoints is empty - Script will try to gather all child objects as waypoints on start
//----------------------------------------------------------------------------------------------using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaypointsHolder : MonoBehaviour 
{
	public Color color = new Color (0, 1, 0, 0.5f); 		// Debug path lines color
	public List<Waypoint> waypoints = new List<Waypoint>();	// List of all waypoints assigned to this path
	public bool colorizeWaypoints = true;   				// Repaint all waypoints in the color


	//=============================================================================================================
	// If list of waypoints is empty - try to gather all child objects(with waypoint script attached) as waypoints
	void Awake () 
	{
		if (waypoints == null  ||  waypoints.Count == 0)
		{
			Waypoint[] childrenWaypoints = GetComponentsInChildren<Waypoint>();
			foreach (Waypoint waypoint in childrenWaypoints) 
				waypoints.Add(waypoint);
		}

		Clean ();
	}

    private void OnEnable()
    {
        Awake();
    }

    //----------------------------------------------------------------------------------
    //Remove missing Waypoints
    public void Clean () 
	{
		for (int i = 0; i < waypoints.Count; i++)
			if (waypoints[i] == null)
			{
				waypoints.RemoveAt (i);
				i--;
			}
	}

	//----------------------------------------------------------------------------------
	// Add existing waypoint to the end of path
	public void AddWaypoint (Waypoint _newWaypoint) 
	{
		waypoints.Add (_newWaypoint);
	}

	//----------------------------------------------------------------------------------
	// Create new waypoint in  specified coordinates and add it to the end of path
	public void CreateWaypoint (Vector3 _position, string name = "waypoint") 
	{
		GameObject newWaypoint = new GameObject();
		newWaypoint.name = name;
		newWaypoint.transform.parent = transform;
		newWaypoint.transform.position = _position;

		waypoints.Add (newWaypoint.AddComponent<Waypoint>());
	}

	//----------------------------------------------------------------------------------
	// Draw debug visualization
	void OnDrawGizmos() 
	{
		Gizmos.color = color;

		if (waypoints.Count > 0)
			for (int i = 0; i<(waypoints.Count-1); i++)
				if (waypoints[i] && waypoints[i+1])  
				{
					Gizmos.DrawLine (waypoints[i].gameObject.transform.position, waypoints[i+1].gameObject.transform.position);
					if (colorizeWaypoints) 
						waypoints[i+1].color = color;
				}
	}

	//----------------------------------------------------------------------------------
}