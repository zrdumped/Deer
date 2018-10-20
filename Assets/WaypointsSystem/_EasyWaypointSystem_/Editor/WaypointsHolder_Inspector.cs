using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(WaypointsHolder))]
public class WaypointsHolder_Inspector : Editor 
{
	WaypointsHolder _target;
	bool creationEnabled = false;
	RaycastHit hit;


	//----------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{		
		_target = (WaypointsHolder)target;

		DrawDefaultInspector ();

		if (Application.isPlaying)  return;


		if (GUILayout.Button (new GUIContent("Clean", "Remove all missing waypoints")))
			_target.Clean ();
		
		if (GUILayout.Button (new GUIContent("Gather child Waypoints", "Reset waypoints list, by gathering only waypoints in children objects"))) 
		{
			_target.waypoints.Clear ();
			foreach (Waypoint waypoint in _target.gameObject.GetComponentsInChildren<Waypoint>())
				_target.waypoints.Add (waypoint);
		}


		GUI.color = creationEnabled ? Color.yellow : Color.white;
		if (GUILayout.Button (new GUIContent("Right mouse to create: " + (creationEnabled ? "ENABLED" : "DISABLED"), "Enable/Disable dynamic waypoints creation by RMB right in Editor Scene View"))) 
			creationEnabled = !creationEnabled;
		
	}

	//----------------------------------------------------------------------------------
	void OnSceneGUI()
    {
		if (creationEnabled)
			if (Event.current.type == EventType.MouseUp  &&  Event.current.button == 1)
  				if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit, 10000))
                {
					_target.CreateWaypoint (hit.point, "waypoint_" + _target.waypoints.Count.ToString());
                    EditorUtility.SetDirty(this);
                }
     
    }

}