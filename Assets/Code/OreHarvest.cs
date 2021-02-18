using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreHarvest
	:
	InteractiveBase
{
	protected override void Start()
	{
		base.Start();

		dropSpot = transform.Find( "DropSpot" );

		harvestRate.Update( harvestRate.GetDuration() );
	}

	protected override void Update()
	{
		base.Update();

		harvestRate.Update( Time.deltaTime );
		nDrops = dropCount.Rand();
	}

	protected override void Interact()
	{
		if( harvestRate.IsDone() )
		{
			// give ore and shake anim

			var drop = Instantiate( dropPrefab );
			drop.transform.position = dropSpot.position;
			drop.GetComponent<LoadableItem>().RandomToss();

			harvestRate.Reset();

			if( --nDrops < 1 ) Destroy( gameObject );
		}
	}

	[SerializeField] Timer harvestRate = new Timer( 1.0f );
	
	[SerializeField] GameObject dropPrefab = null;
	[SerializeField] RangeI dropCount = new RangeI( 1,1 );

	int nDrops;

	Transform dropSpot;
}
