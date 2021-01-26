using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarHandler
	:
	MonoBehaviour
{
	void Start()
	{
		var hotbarObj = GameObject.Find( "HotbarPanel" ).transform;

		for( int i = 0; i < hotbarObj.childCount; ++i )
		{
			invSlots.Add( hotbarObj.GetChild( i ).GetComponent<InventorySlot>() );
		}

		wepHolder = GetComponent<WeaponHolder>();
		fistPrefab = Resources.Load<GameObject>( "Prefabs/Fist" );

		SwapSlot( 0 );
	}

	void Update()
	{
		for( int i = 0; i < invSlots.Count; ++i )
		{
			if( Input.GetKeyDown( KeyCode.Alpha1 + i ) )
			{
				SwapSlot( i );
				break;
			}
		}

		if( Input.mouseScrollDelta.x != 0.0f )
		{
			SwapSlot( curSlot + ( int )Input.mouseScrollDelta.x );
		}
	}

	void SwapSlot( int slot )
	{
		invSlots[curSlot].ToggleActivation( false );
		invSlots[slot].ToggleActivation( true );
		curSlot = slot;

		var wepPrefab = invSlots[curSlot].GetPrefab();
		if( wepPrefab == null ) wepPrefab = fistPrefab;
		wepHolder.ReplaceWeapon( wepPrefab );
	}

	public void RefreshSlot()
	{
		SwapSlot( curSlot );
	}

	List<InventorySlot> invSlots = new List<InventorySlot>();

	int curSlot = 0;

	WeaponHolder wepHolder;
	GameObject fistPrefab;
}
