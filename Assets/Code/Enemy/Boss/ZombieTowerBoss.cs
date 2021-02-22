using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTowerBoss
    :
    BossBase
{
	protected override void Start()
	{
		base.Start();

		boxColl = GetComponent<BoxCollider>();
		exitPortal = ResLoader.Load( "Prefabs/HubPortal" );
	}

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
				if( fireballDuration.Update( Time.deltaTime ) )
				{
					phase = 2;
					// animCtrl.SetBool( "spin",false );
					// animCtrl.SetBool( "walk",true );
					fireballDuration.Reset();
				}
				else
				{
					if( fireballRefire.Update( Time.deltaTime ) )
					{
						fireballRefire.Reset();

						float randY = BoxPointSelector.GetRandPointWithinBox( boxColl ).y;
						var pos = transform.position;
						pos.y = randY;

						pos.x += Random.Range( -1.0f,1.0f );
						pos.z += Random.Range( -1.0f,1.0f );
						
						// float ang = Random.Range( 0.0f,360.0f );
						// var dir = new Vector3( Mathf.Cos( ang ),Mathf.Sin( ang ),0.0f ) * fireballSpeed;
						var targetPos = player.transform.position + new Vector3(
							Random.Range( -1.0f,1.0f ) * fireballSpread.x,
							Random.Range( -1.0f,1.0f ) * fireballSpread.y,
							Random.Range( -1.0f,1.0f ) * fireballSpread.z );
						var dir = targetPos - pos;

						FireProjectile( fireballPrefab,pos,dir.normalized * fireballSpeed );
					}
				}
				break;
			case 2:
				if( spawnDuration.Update( Time.deltaTime ) )
				{
					prevMinion.ActivateSelf();
				
					spawnDuration.Reset();
					phase = 3;

					animCtrl.SetBool( "spin",false );
					animCtrl.SetBool( "walk",true );

					targetSpot = player.transform.position;
				}
				else
				{
					if( spawnRefire.Update( Time.deltaTime ) )
					{
						spawnRefire.Reset();
						var minion = Instantiate( minionPrefab );
						minion.transform.position = transform.position + Vector3.up * 1.0f;
						var jumpDir = new Vector3(
							Random.Range( -1.0f,1.0f ),0.0f,Random.Range( -1.0f,1.0f ) )
							.normalized * minionSpawnHorizontal;
						jumpDir.y = minionSpawnJump * minionSpawnJump;
						minion.GetComponent<Rigidbody>().AddForce( jumpDir,ForceMode.Impulse );
						
						// Activate last minion when we spawn new one so they dont all follow while flying.
						prevMinion?.ActivateSelf();
						prevMinion = minion.GetComponent<EnemyBase>();
						prevMinion.DisableDrops();
					}
				}
				break;
			case 3:
				if( trackingDuration.Update( Time.deltaTime ) )
				{
					trackingDuration.Reset();
					phase = 0;
				}
				else
				{
					if( trackRetarget.Update( Time.deltaTime ) )
					{
						trackRetarget.Reset();
						targetSpot = player.transform.position;
					}

					Move( targetSpot - transform.position );
				}
				break;
		}
	}

	protected override void Oof()
	{
		base.Oof();

		var portal = Instantiate( exitPortal );
		portal.transform.position = transform.position + Vector3.up * 0.2f;
	}

	int phase = 0;

	BoxCollider boxColl;
	GameObject exitPortal;

	[Header( "Fireball Phase" )]
	[SerializeField] float fireballDist = 10.0f;
	[SerializeField] Timer fireballRefire = new Timer( 0.2f );
	[SerializeField] Timer fireballDuration = new Timer( 3.0f );
	[SerializeField] GameObject fireballPrefab = null;
	[SerializeField] float fireballSpeed = 10.0f;
	[SerializeField] Vector3 fireballSpread = Vector3.one;

	[Header( "Spawning Phase" )]
	[SerializeField] Timer spawnDuration = new Timer( 3.0f );
	[SerializeField] Timer spawnRefire = new Timer( 0.5f );
	[SerializeField] GameObject minionPrefab = null;
	[SerializeField] float minionSpawnHorizontal = 5.0f;
	[SerializeField] float minionSpawnJump = 5.0f;
	EnemyBase prevMinion = null;

	[Header( "Slow Tracking Phase" )]
	Vector3 targetSpot = Vector3.zero;
	[SerializeField] Timer trackRetarget = new Timer( 3.0f );
	[SerializeField] Timer trackingDuration = new Timer( 6.0f );
}
