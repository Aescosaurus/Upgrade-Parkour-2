using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyShroom
	:
	MonoBehaviour
{
	void Start()
	{
		particles = GetComponentInChildren<ParticleSystem>();
	}

	void OnTriggerEnter( Collider coll )
	{
		var playerScr = coll.GetComponent<PlayerMove>();
		if( playerScr != null )
		{
			playerScr.ForceMoveCancel( transform.up * bounceForce );
			particles.Emit( particleCount.Rand() );
		}
	}

	ParticleSystem particles;

	[SerializeField] float bounceForce = 1.0f;
	[SerializeField] RangeI particleCount = new RangeI( 10,20 );
}
