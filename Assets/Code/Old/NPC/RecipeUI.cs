using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI
	:
	MonoBehaviour
{
	void Start()
	{
		var ingredientArea = transform.Find( "Ingredients" );
		for( int i = 0; i < ingredientArea.childCount; ++i )
		{
			ingredientSpots.Add( ingredientArea.GetChild( i ).GetComponent<InventorySlot>() );
		}
		resultSpot = transform.Find( "Results" ).GetChild( 0 ).GetComponent<InventorySlot>();

		while( ingredients.Count < 3 ) ingredients.Add( null );
		while( ingredientQuantities.Count < 3 ) ingredientQuantities.Add( 0 );

		for( int i = 0; i < ingredientSpots.Count; ++i )
		{
			if( ingredients[i] != null )
			{
				transform.Find( "Ingredient" + ( i + 1 ).ToString() + "Text" )
					.GetComponent<Text>().text = ingredients[i].GetInvName() +
					" x " + ingredientQuantities[i].ToString();
			}
			else
			{
				ingredientSpots[i].gameObject.SetActive( false );
			}
		}
		if( result != null ) transform.Find( "ResultText" ).GetComponent<Text>().text = result.GetInvName() + " x " + resultQuantity;
	}

	public void TryExchange()
	{
		bool success = true;

		if( resultSpot.CountItems() > 0 ) success = false;

		for( int i = 0; i < ingredientSpots.Count; ++i )
		{
			if( !CheckSpotMatch( ingredients[i],ingredientSpots[i],ingredientQuantities[i] ) )
			{
				success = false;
				break;
			}
		}

		if( success )
		{
			for( int i = 0; i < ingredientSpots.Count; ++i )
			{
				ingredientSpots[i].RemoveItem( ingredientQuantities[i] );
			}

			resultSpot.AddItem( result,resultQuantity );
		}
	}

	bool CheckSpotMatch( LoadableItem desired,InventorySlot slot,int quantity )
	{
		if( desired == null || slot == null ) return( true );
		else return( ( desired.GetSrc() == slot.GetItem().GetSrc() && slot.CountItems() >= quantity ) ||
				( desired.GetComponent<WeaponBase>() != null &&
				slot.GetPrefab().GetComponent<WeaponBase>() != null &&
				desired.GetComponent<PotionBase>() != null &&
				slot.GetPrefab().GetComponent<PotionBase>() != null ) );
	}

	public List<InventorySlot> GetAllItemSlots()
	{
		var itemSlots = new List<InventorySlot>();
		itemSlots.AddRange( ingredientSpots );
		itemSlots.Add( resultSpot );
		return( itemSlots );
	}

	[SerializeField] List<LoadableItem> ingredients = new List<LoadableItem>();
	[SerializeField] List<int> ingredientQuantities = new List<int>();
	[SerializeField] LoadableItem result = null;
	[SerializeField] int resultQuantity = 0;

	List<InventorySlot> ingredientSpots = new List<InventorySlot>();
	InventorySlot resultSpot;
}
