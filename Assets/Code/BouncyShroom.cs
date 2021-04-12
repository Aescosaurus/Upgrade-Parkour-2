using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyShroom
	:
	MonoBehaviour
{
	void OnTriggerEnter( Collider coll )
	{
		var playerScr = coll.GetComponent<PlayerMove>();
		if( playerScr != null )
		{
			playerScr.ApplyForceMove( transform.up * bounceForce );
		}
	}

	[SerializeField] float bounceForce = 1.0f;
}
