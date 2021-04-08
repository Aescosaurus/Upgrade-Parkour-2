using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover
	:
	Activateable
{
	void Start()
	{
		moveDir.Normalize();
	}

	public override void Activate()
	{
		transform.Translate( moveDir * moveSpeed * Time.deltaTime );

		if( despawn.Update( Time.deltaTime ) )
		{
			Destroy( gameObject );
		}
	}

	[SerializeField] float moveSpeed = 1.0f;
	[SerializeField] Vector3 moveDir = Vector3.zero;
	[SerializeField] Timer despawn = new Timer( 5.0f );
}
