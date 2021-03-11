using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase
	:
	Damageable
{
	protected override void Start()
	{
		base.Start();

		// bulletPrefab = Resources.Load<GameObject>( "Prefabs/EnemyBullet" );
		// lobPrefab = Resources.Load<GameObject>( "Prefabs/EnemyLob" );
		// aoePrefab = Resources.Load<GameObject>( "Prefabs/EnemyAOE" );
		// bopperPrefab = Resources.Load<GameObject>( "Prefabs/EnemyBopper" );
		shardPrefab = ResLoader.Load( "Prefabs/Item/MonsterShardSmall" );

		player = FindObjectOfType<PlayerWalk>().gameObject;
		body = GetComponent<Rigidbody>();
		animCtrl = GetComponent<Animator>();

		// partHand = GetComponent<ParticleSystem>();
		// partHand = FindObjectOfType<ParticleHandler>();

		wepHolder = GetComponent<WeaponHolder>();

		partHand.SpawnParticles( transform.position,20,ParticleHandler.ParticleType.Smoke );

		bulletLayer = LayerMask.NameToLayer( "EnemyBullet" );
	}

	protected override void Update()
	{
		base.Update();

		wepHolder?.SetTargetDir( transform.eulerAngles.y );

		if( IsWithinActivateRange( player ) )
		{
			if( !activated )
			{
				ActivateSelf();
				ActivateNearby();
			}
		}

		body.velocity += Vector3.down * gravAcc * Time.deltaTime;
	}

	// public override void Damage( float amount )
	// {
	// 	base.Damage( amount );
	// 
	// 	// partHand.Emit( ( int )( ( amount + 0.5f ) * 15.0f ) );
	// 	partHand.SpawnParticles( transform.position,( int )( ( amount + 0.5f ) * 15.0f ),ParticleHandler.ParticleType.Ouch );
	// }

	protected virtual void Move( Vector3 dir )
	{
		Strafe( dir );

		var rot = transform.eulerAngles;
		rot.y = Mathf.Atan2( dir.x,dir.z ) * Mathf.Rad2Deg;
		rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
		transform.eulerAngles = rot;
		// body.velocity = dir.normalized * moveSpeed;

		animCtrl.SetBool( "walk",true );
	}

	protected virtual void Strafe( Vector3 dir )
	{
		SetVel( dir.normalized * moveSpeed );
	}

	protected virtual void StopMoving()
	{
		// body.velocity = Vector3.zero;
		SetVel( Vector3.zero );

		animCtrl.SetBool( "walk",false );
	}

	protected void Look( Vector3 dir )
	{
		dir.y = 0.0f;
		if( dir.sqrMagnitude > 0.0f )
		{
			transform.forward = dir;
		}
	}

	protected GameObject FireProjectile( GameObject prefab,Vector3 pos,Vector3 aim )
	{
		var proj = Instantiate( prefab );

		var projScr = proj.GetComponent<Projectile>();

		proj.GetComponent<Collider>().isTrigger = true;
		proj.transform.position = pos;
		proj.transform.forward = aim;
		proj.GetComponent<Rigidbody>().AddForce( aim.normalized * projScr.GetShotSpd(),ForceMode.Impulse );
		proj.layer = bulletLayer;

		proj.GetComponent<Projectile>().SetTeam( GetTeam() );

		Destroy( proj.GetComponent<LoadableItem>() );
		Destroy( proj.GetComponent<ItemPickup>() );

		return ( proj );
	}

	// protected void Fire( Vector3 dir )
	// {
	// 	var bullet = Instantiate( bulletPrefab );
	// 	bullet.transform.position = transform.position;
	// 	// bullet.transform.forward = dir;
	// 	// bullet.GetComponent<Rigidbody>().AddForce( dir.normalized * shotSpeed,ForceMode.Impulse );
	// 	bullet.GetComponent<EnemyBulletBase>().Fire( dir );
	// }
	// 
	// protected void Lob( Vector3 target )
	// {
	// 	var bullet = Instantiate( lobPrefab );
	// 	bullet.transform.position = transform.position;
	// 	var lob = bullet.GetComponent<EnemyLob>();
	// 	lob.explosionPrefab = aoePrefab;
	// 	lob.Toss( target );
	// }
	// 
	// protected void SpawnBopper( Vector3 dir )
	// {
	// 	var bullet = Instantiate( bopperPrefab );
	// 	bullet.transform.position = transform.position;
	// 	bullet.GetComponent<EnemyBulletBase>().Fire( new Vector3( dir.x,0.0f,dir.z ) );
	// }

	protected void Attack()
	{
		wepHolder.TryAttack( transform.eulerAngles.y );
	}

	protected void CancelAttack()
	{
		wepHolder.CancelAttack();
	}

	protected bool IsWithinActivateRange( GameObject target )
	{
		return( IsWithinRangeOf( target,activationRange ) );
	}

	protected bool IsWithinRangeOf( GameObject target,float range )
	{
		var dist = target.transform.position - transform.position;
		return ( dist.sqrMagnitude < Mathf.Pow( range,2 ) );
	}

	public virtual void ActivateSelf()
	{
		activated = true;
	}

	void ActivateOther()
	{
		activated = true;
	}

	void ActivateNearby()
	{
		foreach( var enemy in FindObjectsOfType<EnemyBase>() )
		{
			if( IsWithinActivateRange( enemy.gameObject ) ) enemy.ActivateOther();
		}
	}

	public override void Damage( float amount )
	{
		base.Damage( amount );

		if( !activated )
		{
			ActivateSelf();
			ActivateNearby();
		}
	}

	protected override void Oof()
	{
		base.Oof();

		if( !spawnedShard ) SpawnShard();
	}

	void SpawnShard()
	{
		var shard = Instantiate( shardPrefab );
		shard.transform.position = transform.position;
		shard.GetComponent<LoadableItem>().RandomToss();

		if( itemDropPrefab != null )
		{
			var item = Instantiate( itemDropPrefab );
			item.transform.position = transform.position;
			item.GetComponent<LoadableItem>().RandomToss();
		}

		spawnedShard = true;
	}

	void SetVel( Vector3 newVel )
	{
		newVel.y = body.velocity.y;
		body.velocity = newVel;
	}

	protected virtual void OnCollisionEnter( Collision coll )
	{
		if( body != null )
		{
			var vel = body.velocity;
			vel.y = 0.0f;
			body.velocity = vel;
		}
	}

	public void DisableDrops()
	{
		// TODO: Loot table object to do this more intelligently.
		spawnedShard = true;
	}

	public virtual void WopEnd()
	{
	}

	// transition from state1 to state2
	protected void Transition( string state1,string state2 )
	{
		animCtrl.SetBool( state1,false );
		animCtrl.SetBool( state2,true );
	}

	protected Vector3 CalcPerp( Vector3 dir )
	{
		return( new Vector3( dir.z,0.0f,-dir.x ) );
	}

	public virtual void AttackStart()
	{

	}

	// [SerializeField] float hp = 10.0f;

	// [SerializeField] GameObject bulletPrefab = null;
	// GameObject bulletPrefab;
	// GameObject lobPrefab;
	// GameObject aoePrefab;
	// GameObject bopperPrefab;
	GameObject shardPrefab;

	protected GameObject player;
	protected Rigidbody body;
	protected Animator animCtrl;

	// ParticleSystem partHand;
	// ParticleHandler partHand;

	protected WeaponHolder wepHolder;
	LayerMask bulletLayer;

	[SerializeField] float activationRange = 15.0f;
	protected bool activated = false;

	[SerializeField] protected float moveSpeed = 1.0f;
	[SerializeField] float rotSpeed = 2.4f;
	const float gravAcc = 15.0f;

	bool spawnedShard = false;

	[SerializeField] GameObject itemDropPrefab = null;
}
