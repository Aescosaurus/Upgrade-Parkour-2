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

		invTutText = transform.parent.Find( "CloseInvText" ).GetComponent<Text>();

		base.Start();
	}

	void Update()
	{
		if( SpiffyInput.CheckAxis( "Inventory" ) ) ToggleOpen( !open );
		if( SpiffyInput.CheckAxis( "Menu" ) ) ToggleOpen( false );
	}

	// void ToggleOpen( bool on )
	// {
	// 	open = on;
	// 	storagePanel.SetActive( on );
	// 	Cursor.visible = on;
	// 	Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
	// }
	public override void ToggleOpen( bool on )
	{
		base.ToggleOpen( on );

		invTutText.enabled = on;
	}

	// true if success false if full
	public bool AddItem( LoadableItem item )
	{
		bool full = true;

		// try stacking item in hotbar, then inventory, before creating new stack
		// if( hotbar.TryStackItem( item ) < 1 || TryStackItem( item ) < 1 )
		// {
		// 	full = false;
		// }
		if( hotbar.CheckExisting( item ) && hotbar.TryStackItem( item ) < 1 ) full = false;
		if( CheckExisting( item ) && TryStackItem( item ) < 1 ) full = false;

		if( full && hotbar.TryAddItem( item ) )
		{
			full = false;
		}

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

	public override bool TryConsumeItem( LoadableItem item,int quantity = 1 )
	{
		bool consumed = false;

		consumed = hotbar.TryConsumeItem( item,quantity );
		if( !consumed ) consumed = base.TryConsumeItem( item,quantity );

		return( consumed );
	}

	// GameObject invPanel;
	// GameObject invSlotPrefab;

	// List<InventorySlot> slots = new List<InventorySlot>();

	HotbarHandler hotbar;

	// bool open = false;
	Text invTutText;
}
