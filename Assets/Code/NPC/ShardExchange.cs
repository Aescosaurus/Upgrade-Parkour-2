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
			// for( int i = 0; i < shardSpot.CountItems(); ++i )
			// {
			// 	coinSpot.AddItem( coinPrefab );
			// 	shardSpot.RemoveItem();
			// }
			coinSpot.AddItem( coinPrefab,shardSpot.CountItems() );
			shardSpot.RemoveItem( shardSpot.CountItems() );
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
