using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase
	:
	MonoBehaviour
{
	void Start()
	{
		Destroy( gameObject,lifetime );
	}

	protected virtual void OnTriggerEnter( Collider coll )
	{
		// destroy gameobject
	}

	public void Fire( Vector3 dir )
	{
		transform.forward = dir;
		GetComponent<Rigidbody>().AddForce( dir.normalized * moveSpeed,ForceMode.Impulse );
	}

	[SerializeField] float lifetime = 5.0f;
	[SerializeField] float moveSpeed = 10.0f;
}
