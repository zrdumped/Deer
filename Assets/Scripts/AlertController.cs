using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertController : MonoBehaviour {

    //private enum AlertTriggerStatus { Patrol, Alert, Missing};
    //private AlertTriggerStatus status = AlertTriggerStatus.Patrol;

    public WaypointsHolder wayPointsHolder;
    public Waypoint characterWayPoint;
    public float alertMovementSpeed = 4;
    public float patrolMovenmentSpeed = 3;
    //public Waypoint leavePosWayPoint;
    private List<Waypoint> patrolWayPoints;
    private WaypointMover waypointMover;
    //private Vector3 leavePosition;

    // Use this for initialization
    void Start () {
        waypointMover = this.GetComponentInChildren<WaypointMover>();
        patrolWayPoints = wayPointsHolder.waypoints;
        waypointMover.movementSpeed = patrolMovenmentSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Alert();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Patrol();
        }

        //if(status == AlertTriggerStatus.Missing && Vector3.Distance(this.transform.position, leavePosWayPoint.transform.position) < 1)
        //{
        //    status = AlertTriggerStatus.Patrol;
        //    waypointMover.enabled = false;
        //    waypointMover.loopingType = WaypointMover.LoopType.Cycled;
        //    wayPointsHolder.waypoints = patrolWayPoints;
        //    waypointMover.enabled = true;
        //}
    }

    public void Alert()
    {
        //status = AlertTriggerStatus.Alert;
        waypointMover.enabled = false;
        waypointMover.loopingType = WaypointMover.LoopType.Once;
        waypointMover.movementSpeed = alertMovementSpeed;
        wayPointsHolder.waypoints = new List<Waypoint> { characterWayPoint };
        //leavePosWayPoint.transform.position = waypointMover.transform.position;
        waypointMover.enabled = true;
    }

    public void Patrol()
    {
        //status = AlertTriggerStatus.Missing;
        waypointMover.enabled = false;
        waypointMover.loopingType = WaypointMover.LoopType.Cycled;
        waypointMover.movementSpeed = patrolMovenmentSpeed;
        wayPointsHolder.waypoints = patrolWayPoints;
        waypointMover.enabled = true;
    }


}
