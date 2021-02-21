using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorUIBase
	:
	MonoBehaviour
{
	public virtual void CloseUI()
	{
		vendor.GetComponent<NPCVendor>().CloseUI();

		var recipes = GetComponentsInChildren<RecipeUI>();
		foreach( var recipe in recipes )
		{
			var recipeSlots = recipe.GetComponent<RecipeUI>().GetAllItemSlots();
			foreach( var item in recipeSlots )
			{
				DropItems( item );
			}
		}
	}

	protected void DropItems( InventorySlot slot )
	{
		var nDrops = slot.CountItems();
		for( int i = 0; i < nDrops; ++i )
		{
			var item = Instantiate( slot.GetPrefab() );
			item.transform.position = vendor.transform.position + vendor.transform.forward + Vector3.up * 0.3f * i;
			slot.RemoveItem();
		}
	}

	public void SetVendor( GameObject vendor )
	{
		this.vendor = vendor;
	}

	GameObject vendor = null;
}
