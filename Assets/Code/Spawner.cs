using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner
	:
	Activateable
{
	void Start()
	{
		spawnPeriod.Update( spawnPeriod.GetDuration() );
	}

	public override void Activate()
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
