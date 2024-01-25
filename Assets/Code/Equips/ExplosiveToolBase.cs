using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosiveToolBase
	:
	ToolBase
{
	protected void CauseExplosion( Vector3 explodePos )
	{
		var explodables = FindObjectsOfType<Explodable>();
		foreach( var ex in explodables )
		{
			if( ( ex.transform.position - explodePos ).sqrMagnitude < explodableHitRange * explodableHitRange )
			{
				ex.Explode( explodeForceMult );
			}
		}

		var interactives = GameObject.FindGameObjectsWithTag( "Interactive" );
		foreach( var interactive in interactives )
		{
			if( ( interactive.transform.position - explodePos ).sqrMagnitude < interactiveHitRange * interactiveHitRange )
			{
				var pushVec = interactive.transform.position - explodePos;
				// interactive.GetComponent<Rigidbody>().AddForce( ( pushVec.normalized / pushVec.magnitude ) * interactiveForceMult +
				// 	Vector3.up * interactiveUpForce,ForceMode.Impulse );
				interactive.GetComponent<Rigidbody>().AddForceAtPosition( ( pushVec.normalized / pushVec.magnitude ) * interactiveForceMult +
					Vector3.up * interactiveUpForce,
					transform.position,
					ForceMode.Impulse );
			}
		}
	}

	[Header( "Explodable" )]
	[SerializeField] float explodeForceMult = 1.0f;
	[SerializeField] float explodableHitRange = 10.0f;

	[SerializeField] float interactiveForceMult = 1.0f;
	[SerializeField] float interactiveHitRange = 10.0f;
	[SerializeField] float interactiveUpForce = 3.0f;
}