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

	[SerializeField] float hp = 10.0f;
	GameObject bulletPrefab;
	[SerializeField] float shotSpeed = 1.0f;
	protected GameObject player;
}
