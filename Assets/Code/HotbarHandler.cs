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
		throwingWeaponPrefab = Resources.Load<GameObject>( "Prefabs/ThrowingWeapon" );

		SwapSlot( 0 );
	}

	void Update()
	{
		// print( GetCurHeldPrefab() );

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

		var itemPrefab = invSlots[curSlot].GetPrefab();
		if( itemPrefab == null ) itemPrefab = fistPrefab;
		else if( itemPrefab.GetComponent<WeaponBase>() == null )
		{
			itemPrefab = throwingWeaponPrefab;
		}
		wepHolder.ReplaceWeapon( itemPrefab );
	}

	public void RefreshSlot()
	{
		SwapSlot( curSlot );
	}

	// true if success false if full
	public bool TryAddItem( LoadableItem item )
	{
		bool full = true;
		foreach( var slot in invSlots )
		{
			if( slot.TrySetItem( item ) )
			{
				full = false;
				RefreshSlot();
				break;
			}
		}

		return( !full );
	}

	// Try to increase item stack, return false if same item not in hotbar.
	public bool TryStackItem( LoadableItem item )
	{
		foreach( var slot in invSlots )
		{
			// if( slot.GetItem() == item )
			if( slot.TrySetItem( item ) )
			{
				// slot.AddItem( item );
				RefreshSlot();
				return( true );
			}
		}

		return( false );
	}

	public void ConsumeHeldItem()
	{
		// todo support for removing only one of stack
		invSlots[curSlot].RemoveItem();
		if( invSlots[curSlot].CountItems() < 1 ) RefreshSlot();
	}

	public GameObject GetCurHeldPrefab()
	{
		return( invSlots[curSlot].GetPrefab() );
	}

	List<InventorySlot> invSlots = new List<InventorySlot>();

	int curSlot = 0;

	WeaponHolder wepHolder;
	GameObject fistPrefab;
	GameObject throwingWeaponPrefab;
}
