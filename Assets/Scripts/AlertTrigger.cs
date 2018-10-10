using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertTrigger : MonoBehaviour {

    private enum AlertTriggerStatus { Patrol, Alert, Missing};
    private AlertTriggerStatus status = AlertTriggerStatus.Patrol;

    public WaypointsHolder wayPointsHolder;
    public Waypoint characterWayPoint;
    public Waypoint leavePosWayPoint;
    private List<Waypoint> patrolWayPoints;
    private WaypointMover waypointMover;
    private Vector3 leavePosition;

    // Use this for initialization
    void Start () {
        waypointMover = this.GetComponentInChildren<WaypointMover>();
        patrolWayPoints = wayPointsHolder.waypoints;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            status = AlertTriggerStatus.Alert;
            waypointMover.enabled = false;
            waypointMover.loopingType = WaypointMover.LoopType.Once;
            wayPointsHolder.waypoints = new List<Waypoint> { characterWayPoint };
            leavePosWayPoint.transform.position = waypointMover.transform.position;
            waypointMover.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            status = AlertTriggerStatus.Missing;
            waypointMover.enabled = false;
            waypointMover.loopingType = WaypointMover.LoopType.Once;
            wayPointsHolder.waypoints = new List<Waypoint> { leavePosWayPoint };
            waypointMover.enabled = true;
        }

        if(status == AlertTriggerStatus.Missing && Vector3.Distance(this.transform.position, leavePosWayPoint.transform.position) < 1)
        {
            status = AlertTriggerStatus.Patrol;
            waypointMover.enabled = false;
            waypointMover.loopingType = WaypointMover.LoopType.Cycled;
            wayPointsHolder.waypoints = patrolWayPoints;
            waypointMover.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "LightBall")
            status = AlertTriggerStatus.Alert;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "LightBall")
            status = AlertTriggerStatus.Missing;
    }


}
