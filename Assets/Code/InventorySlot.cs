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
	void Start()
	{
		itemPos = transform.Find( "ItemPos" );

		uiLayer = LayerMask.NameToLayer( "UI" );

		rect = GetComponent<RectTransform>();
		img = GetComponent<Image>();
	}

	public void AddItem( GameObject prefab )
	{
		heldItem = Instantiate( prefab.transform.GetChild( 0 ).gameObject,itemPos );

		heldItem.transform.localPosition = new Vector3( -16.6f,-16.7f,-0.8f );
		heldItem.transform.localScale *= itemScaleFactor;
		heldItem.transform.localEulerAngles = new Vector3( 39.2f,70.5f,-12.6f );

		var meshRend = heldItem.transform.GetComponentInChildren<MeshRenderer>();
		meshRend.gameObject.layer = uiLayer;
		meshRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
	}


	public void OnBeginDrag( PointerEventData eventData )
	{
		if( heldItem != null )
		{
			home = rect.position;
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
				transform.position = home;
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

	void TransferItem( InventorySlot receiver )
	{
		// receiver.AddItem( itemPos.gameObject );
		heldItem.transform.SetParent( receiver.itemPos,false );
		receiver.heldItem = heldItem;
		heldItem = null;
		// Destroy( heldItem );
	}

	Transform itemPos;
	GameObject heldItem = null;

	RectTransform rect;

	[SerializeField] float itemScaleFactor = 50.0f;

	int uiLayer;

	Vector3 dragOffset = Vector3.zero;
	Vector3 home;

	Image img;
}
