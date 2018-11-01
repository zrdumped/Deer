using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertController : MonoBehaviour {

    public enum AlertTriggerStatus { Patrol, Alert};
    private AlertTriggerStatus status = AlertTriggerStatus.Patrol;

    public WaypointsHolder wayPointsHolder;
    public Waypoint characterWayPoint;
    public float alertMovementSpeed = 4;
    public float patrolMovenmentSpeed = 3;
    public float prepareSecond = 1;
    public float missSecond = 4;

    public ParticleSystem inactivePS;
    public ParticleSystem activePS;
    //public Waypoint leavePosWayPoint;
    private List<Waypoint> patrolWayPoints;
    private WaypointMover waypointMover;
    //private Vector3 leavePosition;

    // Use this for initialization
    void Start () {
        inactivePS.Play(true);
        waypointMover = this.GetComponentInChildren<WaypointMover>();
        patrolWayPoints = wayPointsHolder.waypoints;
        waypointMover.movementSpeed = patrolMovenmentSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (status == AlertTriggerStatus.Patrol)
                Alert();
            else
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
        status = AlertTriggerStatus.Alert;
        activePS.Play(true);
        inactivePS.Stop(true);
        waypointMover.enabled = false;
        waypointMover.loopingType = WaypointMover.LoopType.Once;
        waypointMover.movementSpeed = 0;
        wayPointsHolder.waypoints = new List<Waypoint> { characterWayPoint };
        //leavePosWayPoint.transform.position = waypointMover.transform.position;
        waypointMover.enabled = true;
        StartCoroutine(Prepare());
        StartCoroutine(Miss());
    }

    public void Patrol()
    {
        activePS.Stop(true);
        inactivePS.Play(true);
        status = AlertTriggerStatus.Patrol;
        waypointMover.enabled = false;
        waypointMover.loopingType = WaypointMover.LoopType.Cycled;
        waypointMover.movementSpeed = patrolMovenmentSpeed;
        wayPointsHolder.waypoints = patrolWayPoints;
        waypointMover.enabled = true;
    }

    IEnumerator Prepare ()
    {
        yield return new WaitForSeconds(prepareSecond);
        waypointMover.movementSpeed = alertMovementSpeed;
    }

    IEnumerator Miss()
    {
        yield return new WaitForSeconds(missSecond);
        Patrol();
    }

    public AlertTriggerStatus GetStatus()
    {
        return status;
    }
}
