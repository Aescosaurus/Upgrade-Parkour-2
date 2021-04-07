using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
	:
	MonoBehaviour
{
	void Start()
	{
		spawnPeriod.Update( spawnPeriod.GetDuration() );
	}

	void Update()
	{
		if( spawnPeriod.Update( Time.deltaTime ) )
		{
			spawnPeriod.Reset();

			var obj = Instantiate( spawnPrefab );
			obj.transform.position = transform.position;
		}	
	}

	[SerializeField] Timer spawnPeriod = new Timer( 5.0f );
	[SerializeField] GameObject spawnPrefab = null;
}
