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

	[SerializeField] float lifetime = 5.0f;
}
