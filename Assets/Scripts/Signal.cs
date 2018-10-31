using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponentInChildren<WaypointMover>().IsOnWaypoint())
        {
            this.GetComponentInChildren<WaypointsHolder>().waypoints[0].gameObject.GetComponent<Key>().signalArrived(this.gameObject);
        }

    }
}
