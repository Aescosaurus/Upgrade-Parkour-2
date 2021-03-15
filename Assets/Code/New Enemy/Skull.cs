using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull
    :
    NewEnemyBase
{
	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			var diff = player.transform.position - transform.position;

			Look( player.transform.position - transform.position );
			if( diff.sqrMagnitude < Mathf.Pow( desiredDist,2.0f ) )
			{
				if( refire.Update( Time.deltaTime ) )
				{
					refire.Reset();
			
					Transition( "walk","attack" );
				}
			}
			else
			{
				Move( diff );
			}
		}
	}

	public void AttackStart()
	{
		FireProjectile( bulletPrefab,transform.position + Vector3.down * 0.2f,transform.forward );

		Transition( "attack","walk" );
	}

	[SerializeField] float desiredDist = 10.0f;

	[SerializeField] GameObject bulletPrefab = null;
	[SerializeField] Timer refire = new Timer( 0.7f );
}
