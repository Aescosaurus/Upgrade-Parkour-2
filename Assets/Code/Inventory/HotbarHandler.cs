using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HotbarHandler
	:
	StorageBase
{
	protected override void Start()
	{
		base.Start();

		// var hotbarObj = GameObject.Find( "HotbarPanel" ).transform;

		// for( int i = 0; i < storagePanel.transform.childCount; ++i )
		// {
		// 	slots.Add( storagePanel.transform.GetChild( i ).GetComponent<InventorySlot>() );
		// }

		wepHolder = GameObject.Find( "Player" ).GetComponent<WeaponHolder>();
		fistPrefab = Resources.Load<GameObject>( "Prefabs/Fist" );
		throwingWeaponPrefab = Resources.Load<GameObject>( "Prefabs/ThrowingWeapon" );

		SwapSlot( 0 );
	}

	void Update()
	{
		// print( GetCurHeldPrefab() );

		for( int i = 0; i < slots.Count; ++i )
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
			if( nextSlot < 0 ) nextSlot += slots.Count;
			if( nextSlot >= slots.Count ) nextSlot -= slots.Count;
			SwapSlot( nextSlot );
		}
	}

	// u can never escape the hotbar muahaha
	protected override void ToggleOpen( bool on )
	{
		open = true;
		gameObject.SetActive( true );
	}

	void SwapSlot( int slot )
	{
		Assert.IsTrue( slot >= 0 && slot < slots.Count );

		slots[curSlot].ToggleActivation( false );
		slots[slot].ToggleActivation( true );
		curSlot = slot;

		var itemPrefab = slots[curSlot].GetPrefab();
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
	// public bool TryAddItem( LoadableItem item )
	// {
	// 	bool full = true;
	// 	foreach( var slot in slots )
	// 	{
	// 		if( slot.TrySetItem( item ) )
	// 		{
	// 			full = false;
	// 			RefreshSlot();
	// 			break;
	// 		}
	// 	}
	// 
	// 	return( !full );
	// }
	public override bool TryAddItem( LoadableItem item )
	{
		bool added = base.TryAddItem( item );

		if( added ) RefreshSlot();

		return( added );
	}

	// Try to increase item stack, return false if same item not in hotbar.
	// public bool TryStackItem( LoadableItem item )
	// {
	// 	foreach( var slot in slots )
	// 	{
	// 		// if( slot.GetItem() == item )
	// 		if( slot.TrySetItem( item ) )
	// 		{
	// 			// slot.AddItem( item );
	// 			RefreshSlot();
	// 			return( true );
	// 		}
	// 	}
	// 
	// 	return( false );
	// }
	public override bool TryStackItem( LoadableItem item )
	{
		bool stacked = base.TryStackItem( item );

		if( stacked ) RefreshSlot();

		return( stacked );
	}

	public void ConsumeHeldItem()
	{
		// todo support for removing only one of stack
		slots[curSlot].RemoveItem();
		if( slots[curSlot].CountItems() < 1 ) RefreshSlot();
	}

	public GameObject GetCurHeldPrefab()
	{
		return( slots[curSlot].GetPrefab() );
	}

	int curSlot = 0;

	WeaponHolder wepHolder;
	GameObject fistPrefab;
	GameObject throwingWeaponPrefab;
}
