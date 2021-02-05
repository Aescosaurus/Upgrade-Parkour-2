using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardExchange
    :
    VendorUIBase
{
    void Start()
	{
		shardSpot = transform.Find( "ShardSpot" ).GetComponent<InventorySlot>();
		coinSpot = transform.Find( "CoinSpot" ).GetComponent<InventorySlot>();

		coinPrefab = Resources.Load<GameObject>( "Prefabs/Coin" ).GetComponent<LoadableItem>();
	}

    public void TryExchange()
	{
		if( shardSpot.GetPrefab()?.GetComponent<MonsterShard>() != null )
		{
			// pls do this w/o loop in future
			for( int i = 0; i < shardSpot.CountItems(); ++i )
			{
				coinSpot.AddItem( coinPrefab );
				shardSpot.RemoveItem();
			}
		}
	}

	public override void CloseUI()
	{
		base.CloseUI();

		DropItems( shardSpot );
		DropItems( coinSpot );
	}

	InventorySlot shardSpot;
	InventorySlot coinSpot;

	LoadableItem coinPrefab;
}
