using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBopper
	:
	EnemyBulletBase
{
	void Start()
	{
		bopPrefab = Resources.Load<GameObject>( "Prefabs/EnemyBop" );
	}

	void Update()
	{
		if( bopSpawnRate.Update( Time.deltaTime ) )
		{
			bopSpawnRate.Reset();
			var bop = Instantiate( bopPrefab,transform.position,Quaternion.identity );
		}
	}

	[SerializeField] Timer bopSpawnRate = new Timer( 1.0f );
	GameObject bopPrefab;
}
