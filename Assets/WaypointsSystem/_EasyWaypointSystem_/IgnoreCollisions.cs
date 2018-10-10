//-------------------------------------------------------------------------------
// Simle script to make the GameObject to ignore some list of colliders
//----------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IgnoreCollisions : MonoBehaviour 
{
	public Collider[] ignoreCollisionWith;

	void Start () 
	{
		if (ignoreCollisionWith.Length > 0  &&  GetComponent<Collider>())
			for (int i = 0; i< ignoreCollisionWith.Length; i++)
				Physics.IgnoreCollision(GetComponent<Collider>(), ignoreCollisionWith[i]);
	}

}