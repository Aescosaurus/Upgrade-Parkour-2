﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja
	:
	EnemyBipedBase
{
	protected override void Start()
	{
		base.Start();

		moveDuration.Update( Random.Range( 0.0f,moveDuration.GetDuration() ) );
		shotRefire = new Timer( wepHolder.GetWeapon().GetComponent<WeaponBase>().GetRefireDuration() );
	}

	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			var diff = player.transform.position - transform.position;
			Move( diff );

			if( moveDuration.Update( Time.deltaTime ) )
			{
				Transition( "walk","strike" );
				Attack();

				if( shotRefire.Update( Time.deltaTime ) )
				{
					shotRefire.Reset();
					if( ++curSwipe > swipeCount )
					{
						curSwipe = 0;
						moveDuration.Reset();
					}
				}
			}
			else
			{
				Transition( "strike","walk" );
			}
		}
	}

	Timer shotRefire;
	[SerializeField] Timer moveDuration = new Timer( 1.0f );
	[SerializeField] int swipeCount = 3;
	int curSwipe = 0;
}
