using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable
	:
	MonoBehaviour
{
	void Start()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
	}

	public void Explode( float forceMult = 1.0f )
	{
		var diff = player.transform.position - transform.position;

		player.GetComponent<PlayerMove2>().ApplyForceMove( diff.normalized / diff.magnitude * explodeForce * forceMult );

		PartHand.SpawnParts( transform.position,explodePartCount,PartHand.PartType.ExplodeBarrel );

		if( destroyOnExplode ) Destroy( gameObject );
	}

	GameObject player;

	[SerializeField] float explodeForce = 10.0f;
	[SerializeField] bool destroyOnExplode = true;
	[SerializeField] int explodePartCount = 20;
}
