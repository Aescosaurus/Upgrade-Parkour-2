using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot
	:
	MonoBehaviour
{
	void Start()
	{
		itemPos = transform.Find( "ItemPos" );
	}

	public void AddItem( GameObject prefab )
	{
		heldItem = Instantiate( prefab.transform.GetChild( 0 ).gameObject,itemPos );

		heldItem.transform.localScale *= itemScaleFactor;
	}

	Transform itemPos;
	GameObject heldItem;

	[SerializeField] float itemScaleFactor = 50.0f;
}
