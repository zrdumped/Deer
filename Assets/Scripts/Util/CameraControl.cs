using UnityEngine;
using System.Collections;

public class CameraControl: MonoBehaviour {

    public Transform target;
    private int MouseWheelSensitivity = 1;
    private int MouseZoomMin = 1;
    private int MouseZoomMax = 5;
    private float normalDistance = 3;

    private Vector3 normalized;

    private float xSpeed = 250.0f;
    private float ySpeed = 120.0f;

    private int yMinLimit = -20;
    private int yMaxLimit = 80;

    private float x = 0.0f;
    private float y = 0.0f;

    private Vector3 screenPoint;
    private Vector3 offset;

    private Quaternion rotation = Quaternion.Euler(new Vector3(30f, 0f, 0f));
    private Vector3 CameraTarget;
    void Start() {

        CameraTarget = target.position;

        float z = target.transform.position.z - normalDistance;
        transform.position = rotation * new Vector3(transform.position.x, transform.position.y, z);

        transform.LookAt(target);

        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update() {

        if(Input.GetMouseButton(1)) {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            var rotation = Quaternion.Euler(y, x, 0);
            var position = rotation * new Vector3(0.0f, 0.0f, -normalDistance) + CameraTarget;

            transform.rotation = rotation;
            transform.position = position;

        }
        else if(Input.GetAxis("Mouse ScrollWheel") != 0) {
            normalized = (transform.position - CameraTarget).normalized;

            if(normalDistance >= MouseZoomMin && normalDistance <= MouseZoomMax) {
                normalDistance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
            }
            if(normalDistance < MouseZoomMin) {
                normalDistance = MouseZoomMin;
            }
            if(normalDistance > MouseZoomMax) {
                normalDistance = MouseZoomMax;
            }
            transform.position = normalized * normalDistance;

        }
        else if(Input.GetMouseButtonDown(2)) {
            screenPoint = Camera.main.WorldToScreenPoint(target.transform.position);
            offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }

        if(Input.GetMouseButton(2)) {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            target.transform.position = curPosition;
        }
        transform.LookAt(CameraTarget);

    }

    static float ClampAngle(float angle, float min, float max) {
        if(angle < -360)
            angle += 360;
        if(angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}