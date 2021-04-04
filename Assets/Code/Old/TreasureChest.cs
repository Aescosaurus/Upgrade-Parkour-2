using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest
	:
	NPCVendor
{
	[System.Serializable]
	class ChestItem
	{
		[SerializeField] public LoadableItem item = null;
		[SerializeField] public RangeI amount = new RangeI( 1,10 );
	}

	protected override void Start()
	{
		base.Start();

		for( int i = 0; i < nSlots; ++i )
		{
			items.Add( null );
			quantities.Add( 0 );
		}

		var nStacks = nItems.Rand();
		for( int i = 0; i < nStacks; ++i )
		{
			int randSlot = -1;
			do
			{
				randSlot = Random.Range( 0,nSlots );
			}
			while( items[randSlot] != null );

			var drop = dropPool[Random.Range( 0,dropPool.Count )];
			items[randSlot] = drop.item;
			quantities[randSlot] = drop.amount.Rand();
		}

		partHand = FindObjectOfType<ParticleHandler>();
	}

	protected override void OpenUI()
	{
		base.OpenUI();

		curUI.GetComponent<TreasureChestUI>().SetItems( items,quantities );
	}

	public override void CloseUI()
	{
		base.CloseUI();

		var slots = curUI.GetComponent<TreasureChestUI>().GetInvSlots();
		for( int i = 0; i < slots.Count; ++i )
		{
			// if( slots[i].CountItems() > 0 )
			{
				// if( items[i] == null ) items[i] = dropPool[0].item;

				// items[i].Copy( slots[i].GetItem() );
				items[i] = slots[i].GetPrefab()?.GetComponent<LoadableItem>();
				quantities[i] = slots[i].CountItems();
			}
		}

		partHand.SpawnParticles( transform.position,15,ParticleHandler.ParticleType.Smoke );

		Destroy( gameObject );
	}

	ParticleHandler partHand;

	List<LoadableItem> items = new List<LoadableItem>();
	List<int> quantities = new List<int>();

	[SerializeField] int nSlots = 30;

	// How many unique item stacks in the chest.
	[SerializeField] RangeI nItems = new RangeI( 1,7 );

	[SerializeField] List<ChestItem> dropPool = new List<ChestItem>();
}
