using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLob
	:
	EnemyBulletBase
{
	protected override void OnTriggerEnter( Collider coll )
	{
		// todo check using layers
		if( coll.GetComponent<EnemyBase>() == null &&
			coll.GetComponent<EnemyBulletBase>() == null )
		{
			if( explosionPrefab != null )
			{
				var explosion = Instantiate( explosionPrefab );
				explosion.transform.position = transform.position;
			}
		
			Destroy( gameObject );
		}
	}

	public void Toss( Vector3 target )
	{
		var moveDir = target - transform.position;
		moveDir.Normalize();
		moveDir.x *= tossPower;
		moveDir.z *= tossPower;
		moveDir.y = tossHeight;
		GetComponent<Rigidbody>().AddForce( moveDir,ForceMode.Impulse );
	}

	[SerializeField] public GameObject explosionPrefab = null;
	[SerializeField] float tossPower = 5.0f;
	[SerializeField] float tossHeight = 5.0f;
}
