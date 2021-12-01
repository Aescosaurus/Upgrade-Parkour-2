using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy
	:
	EnemyAI
{
	protected override void InitAI()
	{
		base.InitAI();

		Retarget();
	}

	protected override void UpdateAI()
	{
		base.UpdateAI();

		var diff = moveTarget - transform.position;
		if( diff.sqrMagnitude < Mathf.Pow( targetResetDist,2 ) )
		{
			Retarget();
		}

		if( targetReset.Update( Time.deltaTime ) )
		{
			Retarget();
		}

		if( body.velocity.sqrMagnitude < maxSpd * maxSpd ||
			Vector3.Dot( diff,body.velocity ) < 0.0f )
		{
			body.AddForce( diff * accel * Time.deltaTime );
		}
	}

	void Retarget()
	{
		targetReset.Reset();

		var targetOffset = new Vector3(
			Random.Range( -1.0f,1.0f ),
			Random.Range( -1.0f,1.0f ),
			Random.Range( -1.0f,1.0f ) ).normalized;
		moveTarget = player.transform.position + targetOffset * targetOffsetDist.Rand();
	}

	Vector3 moveTarget = Vector3.zero;
	float targetResetDist = 1.0f;

	[SerializeField] Timer targetReset = new Timer( 3.0f );
	[SerializeField] RangeF targetOffsetDist = new RangeF( 0.5f,5.0f );
	[SerializeField] float accel = 500.0f;
	[SerializeField] float maxSpd = 10.0f;
}
