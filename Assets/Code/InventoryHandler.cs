using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler
	:
	StorageBase
{
	protected override void Start()
	{
		base.Start();

		// invPanel = GameObject.Find( "InventoryPanel" );

		// for( int i = 0; i < storagePanel.transform.childCount; ++i )
		// {
		// 	slots.Add( storagePanel.transform.GetChild( i ).GetComponent<InventorySlot>() );
		// }

		hotbar = FindObjectOfType<HotbarHandler>();

		// var swordPrefab = Resources.Load<GameObject>( "Prefabs/BasicSword" );
		// slots[0].AddItem( swordPrefab );
		// slots[0].AddItem( Resources.Load<GameObject>( "Prefabs/MonsterShardSmall" ) );

		// ToggleOpen( false );
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Tab ) )
		{
			ToggleOpen( !open );
		}
	}

	// void ToggleOpen( bool on )
	// {
	// 	open = on;
	// 	storagePanel.SetActive( on );
	// 	Cursor.visible = on;
	// 	Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
	// }

	// true if success false if full
	public bool AddItem( LoadableItem item )
	{
		bool full = true;

		// try stacking item in hotbar, then inventory, before creating new stack
		if( hotbar.TryStackItem( item ) || TryStackItem( item ) )
		{
			full = false;
		}

		if( full && hotbar.TryAddItem( item ) ) full = false;

		if( full )
		{
			// foreach( var slot in slots )
			// {
			// 	if( slot.TrySetItem( item ) )
			// 	{
			// 		full = false;
			// 		break;
			// 	}
			// }
			full = TryAddItem( item );
		}

		return( !full );
	}

	// GameObject invPanel;
	// GameObject invSlotPrefab;

	// List<InventorySlot> slots = new List<InventorySlot>();

	HotbarHandler hotbar;

	// bool open = false;
}
