using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemPickup
	:
	MonoBehaviour
{

	void Start()
	{
		itemPrefab = GetComponent<LoadableItem>();

		gameObject.layer = LayerMask.NameToLayer( "ItemPickup" );
		player = FindObjectOfType<PlayerWalk>().gameObject;
	}

	void Update()
	{
		if( ( player.transform.position - transform.position ).sqrMagnitude < Mathf.Pow( pickupDist,2 ) )
		{
			player.GetComponent<PlayerInventory>().GetInv().AddItem( itemPrefab );
			Destroy( gameObject );
		}
	}

	LoadableItem itemPrefab;
	[SerializeField] float pickupDist = 4.0f;
	GameObject player;
}