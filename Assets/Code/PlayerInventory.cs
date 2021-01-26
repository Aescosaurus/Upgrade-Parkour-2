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

		var swordPrefab = Resources.Load<GameObject>( "Prefabs/BasicSword" );
		slots[0].AddItem( swordPrefab );

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

	public bool AddItem( GameObject prefab )
	{
		bool full = true;
		foreach( var slot in slots )
		{
			if( slot.TrySetItem( prefab ) )
			{
				full = false;
				break;
			}
		}

		return( full ); // todo return false if full
	}

	public bool IsOpen()
	{
		return( open );
	}

	GameObject invPanel;
	GameObject invSlotPrefab;

	List<InventorySlot> slots = new List<InventorySlot>();

	bool open = false;
}
