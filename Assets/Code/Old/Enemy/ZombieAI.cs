using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI
	:
	EnemyBipedBase
{
	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			var dir = player.transform.position - transform.position;
			if( dir.sqrMagnitude > stopDist * stopDist )
			{
				dir.y = 0.0f;
				Move( dir );
			}
			else
			{
				StopMoving();
				Look( dir );
				Attack();
			}
		}
	}

	[SerializeField] float stopDist = 1.5f;
}
