using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalWormAI
	:
	EnemyBase
{
	protected override void Start()
	{
		base.Start();

		startLoc = transform.position;

		moveDir = new Vector3(
			Random.Range( -1.0f,1.0f ),
			0.0f,
			Random.Range( -1.0f,1.0f ) );

		hitbox.enabled = false;
	}

	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			if( !hitbox.enabled )
			{
				if( warningDuration.Update( Time.deltaTime ) )
				{
					animCtrl.SetFloat( "walk_spd",chaseAnimSpeedup );
					animCtrl.SetBool( "walk",true );
					animCtrl.SetBool( "spin",false );

					moveSpeed = chaseSpeed;

					var diff = player.transform.position - transform.position;
					diff.y = 0.0f;
					Move( diff );

					if( IsWithinRangeOf( player,attackRange ) )
					{
						hitbox.enabled = true;
						animCtrl.SetBool( "walk",false );
						animCtrl.SetBool( "wop",true );
					}
				}
			}
		}
		else Wander();
	}

	void Wander()
	{
		if( moveRefire.Update( Time.deltaTime ) )
		{
			if( moveChill.Update( Time.deltaTime ) )
			{
				moveRefire.Reset();
				moveChill.Reset();

				moveDir = ( startLoc - transform.position ).normalized;
				moveDir.y = 0.0f;
				moveDir.x += Random.Range( -1.0f,1.0f ) * 0.3f;
				moveDir.z += Random.Range( -1.0f,1.0f ) * 0.3f;
			}
		}
		else Move( moveDir );
	}

	public override void ActivateSelf()
	{
		base.ActivateSelf();

		animCtrl.SetBool( "walk",false );
		animCtrl.SetBool( "spin",true );
	}

	public override void WopEnd()
	{
		base.WopEnd();

		hitbox.enabled = false;
		appliedDamage = false;
		// warningDuration.Reset();
		animCtrl.SetBool( "walk",true );
		animCtrl.SetBool( "wop",false );
	}

	void OnTriggerEnter( Collider coll )
	{
		if( !appliedDamage )
		{
			var damageable = coll.GetComponent<DamageablePlayer>();
			if( damageable != null )
			{
				damageable.Damage( wopDamage );
				appliedDamage = true;
			}
		}
	}

	Vector3 startLoc;
	
	[SerializeField] Timer moveRefire = new Timer( 2.0f );
	[SerializeField] Timer moveChill = new Timer( 0.6f );

	Vector3 moveDir;

	[SerializeField] Timer warningDuration = new Timer( 1.5f );
	[SerializeField] float chaseAnimSpeedup = 2.0f;
	[SerializeField] float chaseSpeed = 3.0f;

	[SerializeField] float attackRange = 3.0f;

	[SerializeField] BoxCollider hitbox = null;

	[SerializeField] float wopDamage = 1.0f;
	bool appliedDamage = false;
}
