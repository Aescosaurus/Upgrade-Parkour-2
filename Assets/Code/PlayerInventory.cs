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
	}

	public bool IsOpen()
	{
		return( true );
	}

	GameObject invPanel;
	GameObject invSlotPrefab;

	List<InventorySlot> slots = new List<InventorySlot>();
}
