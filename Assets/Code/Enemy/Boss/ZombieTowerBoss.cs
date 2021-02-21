using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTowerBoss
    :
    BossBase
{
	protected override void Update()
	{
		base.Update();

		switch( phase )
		{
			case 0:
				{
					var diff = player.transform.position - transform.position;
					diff.y = 0.0f;
					Move( diff );
					if( diff.sqrMagnitude < Mathf.Pow( fireballDist,2 ) )
					{
						StopMoving();
						phase = 1;
						animCtrl.SetBool( "spin",true );
					}
				}
				break;
			case 1:

				break;
		}
	}

	int phase = 0;

	[Header( "Fireball Phase" )]
	[SerializeField] float fireballDist = 10.0f;
	[SerializeField] float fireballRotSpeed = 30.0f;
	[SerializeField] Timer fireballRefire = new Timer( 0.2f );
	[SerializeField] Timer fireballDuration = new Timer( 3.0f );
}
