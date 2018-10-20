using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MalbersAnimations.Utilities
{
    public static class MalbersTools
    {
        /// <summary>
        /// Calculate a Direction from an origin to a target
        /// </summary>
        /// <param name="origin">The start point to calculate the direction</param>
        /// <param name="Target">The Target</param>
        /// <returns></returns>
        public static Vector3 DirectionTarget(Transform origin, Transform Target, bool normalized = true)
        {
            if (normalized)
                return (Target.position - origin.position).normalized;

            return (Target.position - origin.position);
        }


        /// <summary>
        /// Gets the horizontal angle between two vectors. The calculation
        /// removes any y components before calculating the angle.
        /// </summary>
        /// <returns>The signed horizontal angle (in degrees).</returns>
        /// <param name="From">Angle representing the starting vector</param>
        /// <param name="To">Angle representing the resulting vector</param>
        public static float HorizontalAngle(Vector3 From, Vector3 To, Vector3 Up)
        {
            float lAngle = Mathf.Atan2(Vector3.Dot(Up, Vector3.Cross(From, To)), Vector3.Dot(From, To));
            lAngle *= Mathf.Rad2Deg;

            if (Mathf.Abs(lAngle) < 0.0001f) { lAngle = 0f; }

            return lAngle;
        }

        /// <summary>
        /// Calculate the direction from the center of the Screen
        /// </summary>
        /// <param name="origin">The start point to calculate the direction</param>
        ///  <param name="hitmask">Just use this layers</param>
        public static Vector3 DirectionFromCamera(Transform origin, float x, float y, out RaycastHit hit, LayerMask hitmask)
        {
            Camera cam = Camera.main;

            hit = new RaycastHit();

            Ray ray = cam.ScreenPointToRay(new Vector2(x * cam.pixelWidth, y * cam.pixelHeight));
            Vector3 dir = ray.direction;

            hit.distance = float.MaxValue;

            RaycastHit[] hits;

            hits = Physics.RaycastAll(ray, 100, hitmask);

            foreach (RaycastHit item in hits)
            {
                if (item.transform.root == origin.transform.root) continue; //Dont Hit anything in this hierarchy
                if (Vector3.Distance(cam.transform.position, item.point) < Vector3.Distance(cam.transform.position, origin.position)) continue; //If I hit something behind me skip
                if (hit.distance > item.distance) hit = item;
            }

            if (hit.distance != float.MaxValue)
            {
                dir = (hit.point - origin.position).normalized;
            }

            return dir;
        }

        /// <summary>
        /// Calculate the direction from the ScreenPoint of the Screen and also saves the RaycastHit Info
        /// </summary>
        /// <param name="origin">The start point to calculate the direction</param>
        ///  <param name="hitmask">Just use this layers</param>
        public static Vector3 DirectionFromCamera(Transform origin, Vector3 ScreenPoint, out RaycastHit hit, LayerMask hitmask)
        {
            Camera cam = Camera.main;

            Ray ray = cam.ScreenPointToRay(ScreenPoint);
            Vector3 dir = ray.direction;

            hit = new RaycastHit();
            hit.distance = float.MaxValue;
            hit.point = ray.GetPoint(100);
            RaycastHit[] hits;

            hits = Physics.RaycastAll(ray, 100, hitmask);

            foreach (RaycastHit item in hits)
            {
                if (item.transform.root == origin.transform.root) continue;                                     //Dont Hit anything in this hierarchy
                if (Vector3.Distance(cam.transform.position, item.point) < Vector3.Distance(cam.transform.position, origin.position)) continue; //If I hit something behind me skip
                if (hit.distance > item.distance) hit = item;
            }

            if (hit.distance != float.MaxValue)
            {
                dir = (hit.point - origin.position).normalized;
            }
            return dir;
        }

        /// <summary>
        /// Calculate the direction from the center of the Screen
        /// </summary>
        /// <param name="origin">The start point to calculate the direction</param>
        public static Vector3 DirectionFromCamera(Transform origin)
        {
            RaycastHit p;
            return DirectionFromCamera(origin, 0.5f * Screen.width, 0.5f * Screen.height, out p, -1);
        }


        public static Vector3 DirectionFromCamera(Transform origin, Vector3 ScreenCenter)
        {
            RaycastHit p;
            return DirectionFromCamera(origin, ScreenCenter, out p, -1);
        }


        public static Vector3 DirectionFromCameraNoRayCast(Vector3 ScreenCenter)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(ScreenCenter);

            return ray.direction;
        }


        /// <summary>
        /// The angle between dirA and dirB around axis
        /// </summary>
        public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
        {
            // Project A and B onto the plane orthogonal target axis
            dirA = dirA - Vector3.Project(dirA, axis);
            dirB = dirB - Vector3.Project(dirB, axis);

            // Find (positive) angle between A and B
            float angle = Vector3.Angle(dirA, dirB);

            // Return angle multiplied with 1 or -1
            return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
        }


        /// <summary>
        /// Aligns a transform1 to the position and rotation of a transform2
        /// </summary>
        /// <param name="t1">Transform to Aling</param>
        /// <param name="t2">Transform to Aling to</param>
        /// <param name="time">time for the Alingment</param>
        /// <param name="Position">Will align the Position? </param>
        /// <param name="Rotation">Will align the Rotation? </param>
        /// <param name="curve">Will use a curve?</param>
        /// <returns></returns>
        public static IEnumerator AlignTransformsC(Transform t1, Transform t2, float time, bool Position = true, bool Rotation = true, AnimationCurve curve = null)
        {
            float elapsedTime = 0;

            Vector3 CurrentPos = t1.position;
            Quaternion CurrentRot = t1.rotation;

            while ((time > 0) && (elapsedTime <= time))
            {
                float result = curve != null ? curve.Evaluate(elapsedTime / time) : elapsedTime / time;               //Evaluation of the Pos curve


              if (Position)  t1.position = Vector3.LerpUnclamped(CurrentPos, t2.position, result);
              if (Rotation) t1.rotation = Quaternion.SlerpUnclamped(CurrentRot, t2.rotation, result);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            if (Position) t1.position = t2.position;
            if (Rotation) t1.rotation = t2.rotation;
        }

        public static IEnumerator AlignLookAtTransform(Transform t1, Transform t2, float time, AnimationCurve curve = null)
        {
            float elapsedTime = 0;

            Quaternion CurrentRot = t1.rotation;
            Vector3 direction = (t2.position - t1.position).normalized;
            direction.y = t1.forward.y;
            Quaternion FinalRot = Quaternion.LookRotation(direction);
            while ((time > 0) && (elapsedTime <= time))
            {
                float result = curve != null ? curve.Evaluate(elapsedTime / time) : elapsedTime / time;               //Evaluation of the Pos curve

                t1.rotation = Quaternion.SlerpUnclamped(CurrentRot, FinalRot, result);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            t1.rotation = FinalRot;
        }

    }
}