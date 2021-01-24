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

		var dir = player.transform.position - transform.position;
		dir.y = 0.0f;
		Move( dir );
	}
}
