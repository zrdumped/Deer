using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class TPPCamera : MonoBehaviour {

    public bool enableControl = true;


    public float mouseSensitivity = 10.0f;
    public Transform target;
    //public float dstFromTarget = 2.0f;
    public float pitchMin = -40.0f;
    public float pitchMax = 85.0f;
    public float rotationSmoothTime = .02f;
    public float zoomSmoothTime = 10.0f;
    public float zoomInLerpVal = 0.125f;
    public float zoomOutLerpVal = 0.02f;
    public float maxDistance = 3.0f;
    public float adjustmentDistance = 2.0f;
    [System.Serializable]
    public class DebugSettings {
        public bool drawDesiredCollisionLines = false;
        public bool drawAdjustedCollisionLines = true;
    }
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision = new CollisionHandler();



    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    private Vector3 zoomSmoothVelocity;
    private int adHoldLong; // -1--- A hold long; 1--- D hold long;
    private int adHold;
    private float holdTime;
    private Vector3 desiredPosition;
    private float curDistance;
    private bool lockLocation = false;
    private Vector3 lockedLocation = Vector3.zero;
    
    

	// Use this for initialization
	void Start () {
        adHoldLong = 0;
        adHold = 0;
        holdTime = 0;
        collision.Initialize(Camera.main);
        //transform.position = target.position;
        desiredPosition = target.position;
        curDistance = maxDistance;
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(desiredPosition, transform.rotation, ref collision.desiredCameraClipPoints);
    }

    void FixedUpdate() {
        //Debug.Log("holdTime: " + holdTime);
        //Debug.Log("adHold" + adHold);
        //Debug.Log("adHoldLong" + adHoldLong);
        if(enableControl) {
            if(holdTime - 5.0f > 0) {
                adHoldLong = adHold;
            }
            if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) {
                if(adHold != 0) {
                    holdTime += Time.deltaTime;
                }
                return;
            }
            if(Input.GetKey(KeyCode.A)) {
                if(adHold == 1) {
                    adHoldLong = 0;
                    holdTime = 0;
                }
                adHold = -1;
                holdTime += Time.deltaTime;
            }
            else if(Input.GetKey(KeyCode.D)) {
                if(adHold == -1) {
                    adHoldLong = 0;
                    holdTime = 0;
                }
                adHold = 1;
                holdTime += Time.deltaTime;
            }
            if((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                || Input.GetAxis("Mouse X") != 0) {
                adHold = 0;
                adHoldLong = 0;
                holdTime = 0;
            }

            // Code about camera collision
            collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
            collision.UpdateCameraClipPoints(desiredPosition, transform.rotation, ref collision.desiredCameraClipPoints);
            collision.CheckColliding(target.transform.position);
            adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(target.transform.position);

            // Draw debug lines
            for(int i = 0; i < 5; i++) {
                if(debug.drawDesiredCollisionLines) {
                    Debug.DrawLine(target.position, collision.desiredCameraClipPoints[i], Color.white);
                }
                if(debug.drawAdjustedCollisionLines) {
                    Debug.DrawLine(target.position, collision.adjustedCameraClipPoints[i], Color.green);
                }
            }
        }

        if(lockLocation == true) {
            transform.parent.Find("Character").transform.position = lockedLocation;
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        if(enableControl) {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch += Input.GetAxis("Mouse Y") * mouseSensitivity;

            yaw += adHoldLong * 0.05f * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);

            transform.eulerAngles = currentRotation;

            /*
             * 这里对于摄像头位置的移动分为两步：
             * 1）不考虑摄像头的碰撞，距离直接取上一帧的实际距离，先进行摄像头的旋转
             * 2）考虑摄像头的碰撞，按照 collisionHandler 算出的 adjustmentDistance
             * 进行摄像头的 zoomIn/zoomOut
             * 
             * 为什么要分两步：因为摄像头旋转需要加上一些 smooth，摄像头 zoomIn/zoomOut
             * 也需要加上一些 smooth，二这两步的 smooth 力度是不同的，如果合成一步确定位置，
             * 会导致无法满足两步的手感要求。（smooth 过大感觉转动视角粘滞，
             * smooth 过小摄像头弹回太快）
             */
            // 1）先进行摄像头旋转
            transform.position = target.position - transform.forward * curDistance;

            desiredPosition = target.position - transform.forward * maxDistance;

            Vector3 resultPosition;
            if(collision.colliding) {
                resultPosition = target.position - transform.forward * adjustmentDistance;
                curDistance -= zoomInLerpVal * (curDistance - adjustmentDistance);
            }
            else {
                resultPosition = desiredPosition;
                curDistance += zoomOutLerpVal * (maxDistance - curDistance);
            }
            // 2）再进行摄像头拉近拉远
            transform.position = Vector3.SmoothDamp(transform.position, resultPosition, ref zoomSmoothVelocity, zoomSmoothTime);
        }
    }

    [System.Serializable]
    public class CollisionHandler {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam) {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray) {
            if(!camera)
                return;
            intoArray = new Vector3[5];

            // Question：在第三人称摄像机中，FOV 应该是多少？
            // 实测如果 FOV 过大，就算做了相机碰撞检测也会穿模
            // in this case： FOV = 40 比较合适，大于 40 会穿模
            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f); // 3.41f is a magic number.
            float y = x / camera.aspect;

            // Top-left
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;
            // Top-right
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;
            // Bottom-left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
            // Bottom-right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;
            // camera position
            intoArray[4] = cameraPosition - camera.transform.forward * 0.2f; // give us a little bit room behind the camera to collide with.


        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition) {
            for(int i = 0; i < clipPoints.Length; i++) {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                RaycastHit hit;
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if(Physics.Raycast(ray, out hit, distance, collisionLayer)) {
                    if (hit.collider.isTrigger)
                        continue;
                    else
                        return true;
                }
            }
            return false;
        }


        public float GetAdjustedDistanceWithRayFrom(Vector3 from) {
            float distance = -1;
            for(int i = 0; i < desiredCameraClipPoints.Length; i++) {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit)) {
                    if(distance == -1) {
                        distance = hit.distance;
                    }
                    else {
                        if(hit.distance < distance)
                            distance = hit.distance;
                    }
                }
            }

            if(distance == -1) {
                return 0;
            }
            else {
                return distance;
            }
        }

        public void CheckColliding(Vector3 targetPosition) {
            if(CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition)) {
                colliding = true;
                //Debug.Log("collision happen !");
                return;
            }
            else {
                colliding = false;
                //Debug.Log("No collision happen!");
                return;
            }
        }
    }

    public void DisableMove() {
        transform.parent.Find("Character").GetComponent<Animator>().enabled = false;
        lockedLocation = transform.parent.Find("Character").transform.position;
        lockLocation = true;
    }

    public void EnableMove() {
        transform.parent.Find("Character").GetComponent<Animator>().enabled = true;
        lockLocation = false;
    }

    public void DisableCtrl() {
        enableControl = false;
        DisableMove();
    }

    public void EnableCtrl() {
        enableControl = true;
        EnableMove();
    }
}
