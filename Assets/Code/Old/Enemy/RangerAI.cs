using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAI
	:
	EnemyBipedBase
{
	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			minMoveTimer.Update( Time.deltaTime );
			var dir = player.transform.position - transform.position;
			if( noscoping )
			{
				if( noscopeTimer.IsDone() )
				{
					noscoping = false;
					// Look( dir );
					Attack();

					StartCoroutine( LateAttack() );

					noscopeTimer.Reset();
					minMoveTimer.Reset();

					do
					{
						strafeDir = Random.Range( -1,2 );
					}
					while( strafeDir == 0 );
				}

				noscopeTimer.Update( Time.deltaTime );

				var ang = transform.eulerAngles;
				ang.y = Mathf.Lerp( rotStart,rotStart + 360.0f,noscopeTimer.GetPercent() );
				transform.eulerAngles = ang;

				if( noscopeTimer.IsDone() ) Look( dir );
			}
			else if( dir.sqrMagnitude > stopDist * stopDist )
			{
				dir.y = 0.0f;
				Move( dir );
			}
			else
			{
				Look( dir );
				Strafe( CalcStrafeDir( dir ) );
				CancelAttack();
				if( minMoveTimer.IsDone() )
				{
					minMoveTimer.Reset();
					StopMoving();
					Look( dir );
					rotStart = transform.eulerAngles.y;
					noscoping = true;
					body.AddForce( Vector3.up * noscopeJumpForce,ForceMode.Impulse );
					noscopeTimer.Reset();
					// Attack();
					audSrc.PlayOneShot( jumpSound );
				}
			}
		}
	}

	Vector3 CalcStrafeDir( Vector3 dir )
	{
		return( CalcPerp( dir ) * ( float )strafeDir );
	}

	IEnumerator LateAttack()
	{
		yield return( new WaitForSeconds( shotDelay ) );

		var shotPos = transform.position + Vector3.up * 0.8f;
		FireProjectile( projectilePrefab,shotPos,transform.forward );
		FireProjectile( projectilePrefab,shotPos,transform.forward + transform.right * shotSpread );
		FireProjectile( projectilePrefab,shotPos,transform.forward - transform.right * shotSpread );
	}

	[SerializeField] Timer minMoveTimer = new Timer( 2.6f );
	[SerializeField] float stopDist = 10.5f;
	[SerializeField] Timer noscopeTimer = new Timer( 0.5f );
	[SerializeField] float noscopeJumpForce = 3.0f;
	bool noscoping = false;
	float rotStart = 0.0f;
	int strafeDir = -1;

	[SerializeField] GameObject projectilePrefab = null;
	[SerializeField] float shotSpread = 0.3f;
	[SerializeField] float shotDelay = 0.3f;

	[SerializeField] AudioClip jumpSound = null;
}
