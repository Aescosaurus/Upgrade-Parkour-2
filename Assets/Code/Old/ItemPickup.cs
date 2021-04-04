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

		partHand = FindObjectOfType<ParticleHandler>();
	}

	void Update()
	{
		if( ( player.transform.position - transform.position ).sqrMagnitude < Mathf.Pow( pickupDist,2 ) )
		{
			player.GetComponent<PlayerInventory>().GetInv().AddItem( itemPrefab );
			partHand.SpawnParticles( transform.position,10,ParticleHandler.ParticleType.Smoke );
			Destroy( gameObject );
		}
	}

	LoadableItem itemPrefab;
	/*[SerializeField] */float pickupDist = 2.0f;
	GameObject player;
	ParticleHandler partHand;
}