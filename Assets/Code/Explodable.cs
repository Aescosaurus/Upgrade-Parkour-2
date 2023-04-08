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

		var explodePos = transform.position;
		// stolen from ExplosiveToolBase
		var interactives = GameObject.FindGameObjectsWithTag( "Interactive" );
		foreach( var interactive in interactives )
		{
			if( ( interactive.transform.position - explodePos ).sqrMagnitude < interactiveHitRange * interactiveHitRange &&
				interactive != gameObject )
			{
				var pushVec = interactive.transform.position - explodePos;
				interactive.GetComponent<Rigidbody>().AddForce( ( pushVec.normalized / pushVec.magnitude ) * interactiveForceMult +
					Vector3.up * interactiveUpForce,ForceMode.Impulse );
			}
		}

		PartHand.SpawnParts( transform.position,explodePartCount,PartHand.PartType.ExplodeBarrel );

		if( destroyOnExplode ) Destroy( gameObject );
	}

	GameObject player;

	[SerializeField] float explodeForce = 10.0f;
	[SerializeField] bool destroyOnExplode = true;
	[SerializeField] int explodePartCount = 20;

	[Header( "Interactive Explode" )]
	[SerializeField] float interactiveForceMult = 1.0f;
	[SerializeField] float interactiveHitRange = 10.0f;
	[SerializeField] float interactiveUpForce = 3.0f;
}
