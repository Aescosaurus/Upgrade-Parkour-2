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

		uiLayer = LayerMask.NameToLayer( "UI" );
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

	Transform itemPos;
	GameObject heldItem;

	[SerializeField] float itemScaleFactor = 50.0f;

	int uiLayer;
}
