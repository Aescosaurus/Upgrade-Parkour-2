using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

		// print( Input.GetAxis( "Mouse ScrollWheel" ) + " " + Input.mouseScrollDelta.x );
		var scrollAmount = Input.GetAxis( "Mouse ScrollWheel" );
		if( scrollAmount != 0.0f )
		{
			var nextSlot = curSlot - ( int )Mathf.Sign( scrollAmount );
			if( nextSlot < 0 ) nextSlot += invSlots.Count;
			if( nextSlot >= invSlots.Count ) nextSlot -= invSlots.Count;
			SwapSlot( nextSlot );
		}
	}

	void SwapSlot( int slot )
	{
		Assert.IsTrue( slot >= 0 && slot < invSlots.Count );

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

	// true if success false if full
	public bool TryAddItem( GameObject prefab )
	{
		bool full = true;
		foreach( var slot in invSlots )
		{
			if( slot.TrySetItem( prefab ) )
			{
				full = false;
				RefreshSlot();
				break;
			}
		}

		return( !full );
	}

	List<InventorySlot> invSlots = new List<InventorySlot>();

	int curSlot = 0;

	WeaponHolder wepHolder;
	GameObject fistPrefab;
}
