using UnityEngine;
using System.Collections;

namespace NeatWolf.FireBundle01 {
	public class Rotating : MonoBehaviour {
		public Vector3 rotationSpeed = Vector3.zero;
		public Space relativeTo = Space.Self;
		
		// Update is called once per frame
		protected virtual void Update () {
			transform.Rotate(rotationSpeed * Time.deltaTime, relativeTo);
		}

		public virtual void ForcedRotation(float time)
		{
			transform.Rotate(rotationSpeed * time, relativeTo);
		}
	}
}