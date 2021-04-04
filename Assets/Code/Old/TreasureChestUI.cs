using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TreasureChestUI
	:
	StorageBase
{
	public void SetItems( List<LoadableItem> srcItems,List<int> quantities )
	{
		Assert.IsTrue( srcItems.Count <= CountSlots() );
		Assert.IsTrue( srcItems.Count == quantities.Count );

		for( int i = 0; i < srcItems.Count; ++i )
		{
			var curItem = srcItems[i];
			var curAmount = quantities[i];
			if( curAmount > 0 )
			{
				// TryAddItem( curItem );
				// for( int j = 0; j < curAmount - 1; ++j )
				// {
				// 	TryStackItem( curItem );
				// }
				if( curItem is WeaponBase ) curAmount = 1;
				AddInSlot( curItem,curAmount,i );
			}
		}
	}

	void AddInSlot( LoadableItem item,int quantity,int slot )
	{
		Assert.IsTrue( slots[slot].GetItem().GetPrefab() == null );

		slots[slot].AddItem( item,quantity );
	}
}
