using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner
	:
	MonoBehaviour
{
	void Start()
	{
		spawnLoc = transform.Find( "SpawnPoint" );
	}

	void Update()
	{
		if( spawnRate.Update( Time.deltaTime ) )
		{
			spawnRate.Reset();

			if( spawnPrefab != null )
			{
				var curEnemy = Instantiate( spawnPrefab,spawnLoc.transform.position,Quaternion.identity );
				// curEnemy.transform.position = spawnLoc.transform.position;
			}
		}
	}

	[SerializeField] Timer spawnRate = new Timer( 5.0f );
	[SerializeField] GameObject spawnPrefab = null;

	Transform spawnLoc;
}
