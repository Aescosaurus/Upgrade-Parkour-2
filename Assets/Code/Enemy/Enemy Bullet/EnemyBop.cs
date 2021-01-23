using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBop
	:
	EnemyBulletBase
{
	void Start()
	{
		var ray = new Ray( transform.position,-transform.up );
		RaycastHit hit;
		if( !Physics.Raycast( ray,out hit,10.0f,~LayerMask.GetMask( "EnemyBullet" ) ) ) Destroy( gameObject );
		transform.position = hit.point;
	}

	void Update()
	{
		if( activationTime.Update( Time.deltaTime ) )
		{
			activationTime.Reset();

			var scale = transform.localScale;
			scale.y = 2.0f;
			transform.localScale = scale;

			Destroy( gameObject,despawnTime );
		}
	}

	[SerializeField] Timer activationTime = new Timer( 0.7f );
	[SerializeField] float despawnTime = 1.0f;
}
