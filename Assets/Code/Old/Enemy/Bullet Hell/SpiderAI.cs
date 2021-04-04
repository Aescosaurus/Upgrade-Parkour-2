using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAI
    :
    EnemyBase
{
	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			var diff = player.transform.position - transform.position;

			if( strafeTimer.Update( Time.deltaTime ) )
			{
				strafeTimer.Reset();
				strafeDir *= -1.0f;

				FireProjectile( bulletPrefab,transform.position + Vector3.up * 0.4f,diff );
			}

			Look( diff );
			if( diff.sqrMagnitude > Mathf.Pow( minDist,2.0f ) )
			{
				Move( diff );
			}
			else
			{
				Strafe( CalcPerp( diff ) * strafeDir );
			}
		}
	}

	[SerializeField] Timer strafeTimer = new Timer( 2.0f );
	float strafeDir = -1.0f;
	[SerializeField] float minDist = 15.0f;

	[SerializeField] GameObject bulletPrefab = null;
}
