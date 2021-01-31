using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory
	:
	MonoBehaviour
{
	void Start()
	{
		invPanel = GameObject.Find( "InventoryPanel" );

		for( int i = 0; i < invPanel.transform.childCount; ++i )
		{
			slots.Add( invPanel.transform.GetChild( i ).GetComponent<InventorySlot>() );
		}

		hotbar = GetComponent<HotbarHandler>();

		// var swordPrefab = Resources.Load<GameObject>( "Prefabs/BasicSword" );
		// slots[0].AddItem( swordPrefab );
		slots[0].AddItem( Resources.Load<GameObject>( "Prefabs/MonsterShardSmall" ) );

		ToggleOpen( false );
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Tab ) )
		{
			ToggleOpen( !open );
		}
	}

	void ToggleOpen( bool on )
	{
		open = on;
		invPanel.SetActive( on );
		Cursor.visible = on;
		Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
	}

	// true if success false if full
	public bool AddItem( GameObject prefab )
	{
		bool full = true;

		if( hotbar.TryAddItem( prefab ) ) full = false;

		if( full )
		{
			foreach( var slot in slots )
			{
				if( slot.TrySetItem( prefab ) )
				{
					full = false;
					break;
				}
			}
		}

		return( !full );
	}

	public bool IsOpen()
	{
		return( open );
	}

	GameObject invPanel;
	GameObject invSlotPrefab;

	List<InventorySlot> slots = new List<InventorySlot>();

	HotbarHandler hotbar;

	bool open = false;
}
