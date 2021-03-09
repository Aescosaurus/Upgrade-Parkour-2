using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBoss
    :
    BossBase
{
	protected override void Start()
	{
		base.Start();

		wormPrefab = ResLoader.Load( "Prefabs/Enemy/CrystalWorm" );

		hitbox = transform.Find( "Body" ).Find( "Hitbox" ).GetComponent<BoxCollider>();
		hitbox.enabled = false;
	}

	protected override void Update()
	{
		base.Update();

		switch( phase )
		{
			case 0:
				animCtrl.SetBool( "hop",true );
				if( !hopping )
				{
					var diff = player.transform.position - transform.position;
					Look( diff );
				}
				break;
			case 1:
				if( spinDuration.Update( Time.deltaTime ) )
				{
					spinDuration.Reset();
					phase = 2;

					animCtrl.SetBool( "spin",false );
					animCtrl.SetBool( "channel",true );
				}
				else
				{
					if( spinRefire.Update( Time.deltaTime ) )
					{
						spinRefire.Reset();
						for( int i = 0; i < circleSize; ++i )
						{
							float ang = ( ( float )i / ( float )circleSize ) *
								( 360.0f + spinDuration.GetPercent() * angAdd ) *
								Mathf.Deg2Rad;
							var dir = new Vector3(
								Mathf.Cos( ang ),
								0.0f,
								Mathf.Sin( ang ) );
							FireProjectile( projectilePrefab,transform.position + Vector3.up * 0.5f,
								dir.normalized * projectileSpeed );
						}
					}
				}
				break;
			case 2:
				if( spawnDuration.Update( Time.deltaTime ) )
				{
					spawnDuration.Reset();
					phase = 3;
					animCtrl.SetBool( "channel",false );
					animCtrl.SetBool( "hop",true );
				}
				else
				{
					if( spawnRefire.Update( Time.deltaTime ) )
					{
						spawnRefire.Reset();

						var minion = Instantiate( wormPrefab );
						var spawnPos = transform.position + new Vector3(
							Random.Range( -spawnRadius,spawnRadius ),
							spawnHeight,
							Random.Range( -spawnRadius,spawnRadius ) );
						minion.transform.position = spawnPos;
						StartCoroutine( MinionSetup( minion ) );
					}
				}
				break;
			case 3:
				if( wanderDuration.Update( Time.deltaTime ) )
				{
					curHop = 0;
					phase = 0;
				}
				else
				{
					if( wanderReset.Update( Time.deltaTime ) )
					{
						wanderReset.Reset();

						// if( Random.Range( 0.0f,1.0f ) < 0.5f )
						{
							hopDir = new Vector3(
								Random.Range( -1.0f,1.0f ),
								0.0f,
								Random.Range( -1.0f,1.0f ) );
						}
						// else
						// {
						// 	hopDir = player.transform.position - transform.position;
						// }
					}

					animCtrl.SetBool( "hop",true );
					if( !hopping ) Look( hopDir );
				}
				break;
		}
	}

	public void HopStart()
	{
		hopping = true;

		// var diff = player.transform.position - transform.position;
		var diff = transform.forward;
		diff.y = 0.0f;
		body.AddForce( ( diff.normalized + Vector3.up * hopUpBias ).normalized * hopForce,ForceMode.Impulse );

		appliedDamage = false;
		hitbox.enabled = true;
	}

	public void HopEnd()
	{
		hitbox.enabled = false;

		hopping = false;
		++curHop;
		if( phase == 0 && curHop > nHops )
		{
			curHop = 0;
			phase = 1;
			animCtrl.SetBool( "hop",false );
			animCtrl.SetBool( "spin",true );
		}
	}

	protected override void OnCollisionEnter( Collision coll )
	{
		base.OnCollisionEnter( coll );

		// if( hopping )
		// {
		// 	hopping = false;
		// 	++curHop;
		// 	if( phase == 0 && curHop > nHops )
		// 	{
		// 		curHop = 0;
		// 		phase = 1;
		// 		animCtrl.SetBool( "hop",false );
		// 		animCtrl.SetBool( "spin",true );
		// 	}
		// }
	}

	IEnumerator MinionSetup( GameObject minion )
	{
		yield return( new WaitForEndOfFrame() );

		var ai = minion.GetComponent<CrystalWormAI>();
		ai.ActivateSelf();
		ai.DisableDrops();
	}

	void OnTriggerEnter( Collider coll )
	{
		if( !appliedDamage )
		{
			var damageable = coll.GetComponent<DamageablePlayer>();
			if( damageable != null )
			{
				damageable.Damage( hopDamage );
				appliedDamage = true;
			}
		}
	}

	int phase = 0;

	GameObject wormPrefab;
	BoxCollider hitbox;
	bool appliedDamage = false;

	[Header("Hopping Phase" )]
	bool hopping = false;
	[SerializeField] float hopUpBias = 0.3f;
	[SerializeField] float hopForce = 2.0f;
	int curHop = 0;
	[SerializeField] int nHops = 5;
	[SerializeField] float hopDamage = 2.0f;

	[Header( "Spin Cycle" )]
	[SerializeField] Timer spinRefire = new Timer( 0.2f );
	[SerializeField] Timer spinDuration = new Timer( 3.0f );
	[SerializeField] GameObject projectilePrefab = null;
	[SerializeField] float projectileSpeed = 10.0f;
	[SerializeField] int circleSize = 8;
	[SerializeField] float angAdd = 200.0f;

	[Header( "Channeling Phase" )]
	[SerializeField] float spawnHeight = 30.0f;
	[SerializeField] float spawnRadius = 10.0f;
	[SerializeField] Timer spawnRefire = new Timer( 0.4f );
	[SerializeField] Timer spawnDuration = new Timer( 1.7f );

	[Header( "Wander Phase" )]
	[SerializeField] Timer wanderDuration = new Timer( 5.0f );
	[SerializeField] Timer wanderReset = new Timer( 1.0f );
	Vector3 hopDir = Vector3.zero;
}
