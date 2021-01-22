using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase
	:
	MonoBehaviour
{
	void Start()
	{
		bulletPrefab = Resources.Load<GameObject>( "Prefabs/EnemyBullet" );
		lobPrefab = Resources.Load<GameObject>( "Prefabs/EnemyLob" );
		aoePrefab = Resources.Load<GameObject>( "Prefabs/EnemyAOE" );
		player = FindObjectOfType<PlayerWalk>().gameObject;
	}

	public void Damage( float amount )
	{
		hp -= amount;

		if( hp < 0.0f ) Destroy( gameObject );
	}
	
	protected void Fire( Vector3 dir )
	{
		var bullet = Instantiate( bulletPrefab );
		bullet.transform.position = transform.position;
		bullet.transform.forward = dir;
		bullet.GetComponent<Rigidbody>().AddForce( dir.normalized * shotSpeed,ForceMode.Impulse );
	}

	protected void Lob( Vector3 target )
	{
		var bullet = Instantiate( lobPrefab );
		bullet.transform.position = transform.position;
		var lob = bullet.GetComponent<EnemyLob>();
		lob.Toss( target );
		lob.explosionPrefab = aoePrefab;
	}

	[SerializeField] float hp = 10.0f;
	GameObject bulletPrefab;
	GameObject lobPrefab;
	GameObject aoePrefab;
	[SerializeField] float shotSpeed = 1.0f;
	protected GameObject player;
}
