using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		bulletPrefab = Resources.Load<GameObject>( "Prefabs/EnemyBullet" );
		lobPrefab = Resources.Load<GameObject>( "Prefabs/EnemyLob" );
		aoePrefab = Resources.Load<GameObject>( "Prefabs/EnemyAOE" );
		bopperPrefab = Resources.Load<GameObject>( "Prefabs/EnemyBopper" );
		player = FindObjectOfType<PlayerWalk>().gameObject;

		// partHand = GetComponent<ParticleSystem>();
		partHand = FindObjectOfType<ParticleHandler>();

		wepHolder = GetComponent<WeaponHolder>();

		partHand.SpawnParticles( transform.position,20,ParticleHandler.ParticleType.Smoke );
	}

	protected virtual void Update()
	{
		wepHolder.SetTargetDir( transform.eulerAngles.y );
	}

	public void Damage( float amount )
	{
		hp -= amount;

		// partHand.Emit( ( int )( ( amount + 0.5f ) * 15.0f ) );
		partHand.SpawnParticles( transform.position,( int )( ( amount + 0.5f ) * 15.0f ),ParticleHandler.ParticleType.Ouch );

		if( hp <= 0.0f ) Destroy( gameObject );
	}
	
	protected void Fire( Vector3 dir )
	{
		var bullet = Instantiate( bulletPrefab );
		bullet.transform.position = transform.position;
		// bullet.transform.forward = dir;
		// bullet.GetComponent<Rigidbody>().AddForce( dir.normalized * shotSpeed,ForceMode.Impulse );
		bullet.GetComponent<EnemyBulletBase>().Fire( dir );
	}

	protected void Lob( Vector3 target )
	{
		var bullet = Instantiate( lobPrefab );
		bullet.transform.position = transform.position;
		var lob = bullet.GetComponent<EnemyLob>();
		lob.explosionPrefab = aoePrefab;
		lob.Toss( target );
	}

	protected void SpawnBopper( Vector3 dir )
	{
		var bullet = Instantiate( bopperPrefab );
		bullet.transform.position = transform.position;
		bullet.GetComponent<EnemyBulletBase>().Fire( new Vector3( dir.x,0.0f,dir.z ) );
	}

	protected void Attack()
	{
		wepHolder.TryAttack( transform.eulerAngles.y );
	}

	[SerializeField] float hp = 10.0f;

	GameObject bulletPrefab;
	GameObject lobPrefab;
	GameObject aoePrefab;
	GameObject bopperPrefab;
	protected GameObject player;

	// ParticleSystem partHand;
	ParticleHandler partHand;

	WeaponHolder wepHolder;
}
