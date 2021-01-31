using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot
	:
	MonoBehaviour,
	IBeginDragHandler,
	IEndDragHandler,
	IDragHandler
{
	void Awake()
	{
		itemPos = transform.Find( "ItemPos" );

		uiLayer = LayerMask.NameToLayer( "UI" );

		rect = GetComponent<RectTransform>();
		img = GetComponent<Image>();

		ToggleActivation( false );

		counterText = GetComponentInChildren<Text>();
		UpdateCounter();
	}

	void Start()
	{
		hotbar = FindObjectOfType<HotbarHandler>();
	}

	public void AddItem( GameObject prefab )
	{
		++nItems;

		if( heldPrefab == null )
		{
			heldPrefab = prefab;
			heldItem = Instantiate( prefab.transform.GetChild( 0 ).gameObject,itemPos );

			heldItem.transform.localPosition = new Vector3( -16.6f,-16.7f,-0.8f );
			heldItem.transform.localScale *= itemScaleFactor;
			heldItem.transform.localEulerAngles = new Vector3( 39.2f,70.5f,-12.6f );

			var meshRend = heldItem.transform.GetComponentInChildren<MeshRenderer>();
			meshRend.gameObject.layer = uiLayer;
			meshRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		}
		else
		{
			UpdateCounter();
		}
	}

	public void RemoveItem()
	{
		--nItems;
		UpdateCounter();

		if( nItems <= 0 )
		{
			heldPrefab = null;
			Destroy( heldItem );
		}
	}

	public void OnBeginDrag( PointerEventData eventData )
	{
		if( heldItem != null )
		{
			home = rect.localPosition;
			dragOffset = eventData.pointerCurrentRaycast.worldPosition - transform.position;
			img.raycastTarget = false;
		}
	}

	public void OnEndDrag( PointerEventData eventData )
	{
		if( heldItem != null )
		{
			// eventData.pointerEnter
			RectTransform otherItem = eventData.pointerEnter?.GetComponent<RectTransform>();
			var invSlot = otherItem?.GetComponent<InventorySlot>();
			if( otherItem != null && invSlot != null )
			{
				// transform.position = otherItem.position;
				// otherItem.position = home;
				// invSlot.home = home;
				// home = rect.position;
				TransferItem( invSlot );
			}
			// else
			{
				// transform.position = home;
				rect.localPosition = home;
			}

			img.raycastTarget = true;
		}
	}

	public void OnDrag( PointerEventData eventData )
	{
		if( heldItem != null )
		{
			transform.position = eventData.pointerCurrentRaycast.worldPosition - dragOffset;
		}
	}

	// more like swap item
	public void TransferItem( InventorySlot receiver )
	{
		heldItem.transform.SetParent( receiver.itemPos,false );
		receiver.heldItem?.transform.SetParent( itemPos,false );

		var tempHeldItem = heldItem;
		heldItem = receiver.heldItem;
		receiver.heldItem = tempHeldItem;
		// heldItem = null;

		var tempHeldPrefab = heldPrefab;
		heldPrefab = receiver.heldPrefab;
		receiver.heldPrefab = tempHeldPrefab;
		// heldPrefab = null;

		var tempNItems = nItems;
		nItems = receiver.nItems;
		receiver.nItems = tempNItems;
		UpdateCounter();
		receiver.UpdateCounter();

		hotbar.RefreshSlot();
	}

	public void ToggleActivation( bool on )
	{
		var c = img.color;
		c.a = on ? 1.0f : defaultAlpha;
		img.color = c;
	}

	// return true if success in setting item, false if already full
	public bool TrySetItem( GameObject prefab )
	{
		// if( heldItem != null ) print( prefab.name + " " + heldPrefab.name );
		// weps are not stackable
		if( heldItem != null && ( prefab.name != heldPrefab.name || heldItem.GetComponent<WeaponBase>() != null ) ) return( false );

		AddItem( prefab );

		return( true );
	}

	void UpdateCounter()
	{
		counterText.text = "";
		if( nItems > 1 ) counterText.text = nItems.ToString();
	}

	public GameObject GetPrefab()
	{
		return( heldPrefab );
	}

	public int CountItems()
	{
		return( nItems );
	}

	Transform itemPos;
	GameObject heldItem = null;
	[SerializeField] GameObject heldPrefab = null;

	RectTransform rect;

	[SerializeField] float itemScaleFactor = 50.0f;
	[Range( 0.0f,1.0f )]
	[SerializeField] float defaultAlpha = 0.5f;

	int uiLayer;

	Vector3 dragOffset = Vector3.zero;
	Vector3 home;

	Image img;

	HotbarHandler hotbar;

	Text counterText;
	int nItems = 0;
}
