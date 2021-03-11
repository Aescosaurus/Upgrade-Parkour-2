using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAI
	:
	EnemyBase
{
	protected override void Update()
	{
		base.Update();

		var diff = player.transform.position - transform.position;
		
		Look( diff );
		if( diff.sqrMagnitude < Mathf.Pow( desiredDist,2.0f ) )
		{
			// Move( -diff );

			if( refire.Update( Time.deltaTime ) )
			{
				refire.Reset();

				Transition( "walk","attack" );
			}
		}
		else Move( diff );
	}

	public override void AttackStart()
	{
		base.AttackStart();

		FireProjectile( bulletPrefab,transform.position + Vector3.up * 0.2f,transform.forward );

		Transition( "attack","walk" );
	}

	[SerializeField] float desiredDist = 10.0f;

	[SerializeField] float shotRefire = 2.0f;
	[SerializeField] GameObject bulletPrefab = null;
	[SerializeField] Timer refire = new Timer( 0.7f );
}
