﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAI
	:
	EnemyBase
{
	protected override void Start()
	{
		base.Start();

		hitbox = transform.Find( "Hitbox" ).GetComponent<BoxCollider>();
		hitbox.enabled = false;
	}

	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			var dir = player.transform.position - transform.position;
			dir.y = 0.0f;
			if( !lunging )
			{
				if( dir.sqrMagnitude > strafeDist * strafeDist )
				{
					Move( dir );
				}
				else if( dir.sqrMagnitude < fleeDist * fleeDist )
				{
					Move( -dir );
				}
				else
				{
					if( strafeDir == 0 ) strafeDir = Random.Range( -1,2 );

					Move( CalcMoveDir( dir ) );
					Look( dir );

					if( lungeTimer.Update( Time.deltaTime ) )
					{
						lunging = true;
						lungeTimer.Reset();
						animCtrl.SetBool( "walk",false );
						animCtrl.SetBool( "lunging",true );
						appliedDamage = false;
					}
				}
			}
			else
			{
				Look( dir );
			}
		}
	}

	Vector3 CalcMoveDir( Vector3 orig )
	{
		if( strafeDir > 0 ) return( new Vector3( orig.z,0.0f,-orig.x ) );
		else if( strafeDir < 0 ) return( new Vector3( -orig.z,0.0f,orig.x ) );
		else return( orig );
	}

	// Call when wind up is done and ready to apply force.
	public void BeginLunge()
	{
		transform.position += Vector3.up * 0.2f;
		body.AddForce( ( ( player.transform.position - transform.position ).normalized + Vector3.up * lungeUpBias ) * lungePower,
			ForceMode.Impulse );
		hitbox.enabled = true;

		audSrc.PlayOneShot( jumpSound );
	}

	protected override void OnCollisionEnter( Collision coll )
	{
		base.OnCollisionEnter( coll );

		if( animCtrl != null )
		{
			if( lunging )
			{
				strafeDir = Random.Range( -1,2 );
				hitbox.enabled = false;
			}

			lunging = false;
			animCtrl.SetBool( "lunging",false );
		}
	}

	void OnTriggerEnter( Collider coll )
	{
		if( !appliedDamage )
		{
			var damageable = coll.GetComponent<DamageablePlayer>();
			if( damageable != null )
			{
				damageable.Damage( lungeDamage );
				appliedDamage = true;
			}
		}
	}

	[SerializeField] float strafeDist = 5.0f;
	[SerializeField] float fleeDist = 3.0f;
	int strafeDir = 0;
	[SerializeField] Timer lungeTimer = new Timer( 2.0f );
	bool lunging = false;
	[SerializeField] float lungePower = 5.0f;
	[SerializeField] float lungeUpBias = 0.2f;
	BoxCollider hitbox;
	[SerializeField] float lungeDamage = 2.0f;
	bool appliedDamage = false;

	[SerializeField] AudioClip jumpSound = null;
}
