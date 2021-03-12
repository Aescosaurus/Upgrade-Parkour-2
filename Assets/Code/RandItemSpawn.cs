using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandItemSpawn
	:
	MonoBehaviour
{
	void Start()
	{
		if( Random.Range( 0.0f,1.0f ) < spawnChance )
		{
			var drop = Instantiate( possibleDrops[Random.Range( 0,possibleDrops.Count )] );
			drop.transform.position = transform.position + Vector3.up * spawnHeight;
		}
	}

	[SerializeField] List<ItemPickup> possibleDrops = new List<ItemPickup>();
	[SerializeField] float spawnHeight = 1.0f;
	[SerializeField] float spawnChance = 0.2f;
}
