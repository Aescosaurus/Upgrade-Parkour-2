using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponPickup
	:
	InteractiveBase
{
	// void Start()
	// {
	// 	Assert.IsTrue( pickupPrefab != null );
	// 	pickup = Instantiate( pickupPrefab.transform.GetChild( 0 ).gameObject,transform );
	// 	// pickup.transform.Rotate( Vector3.forward,90.0f );
	// 	pickup.transform.position = transform.Find( "WeaponHoldPos" ).position;
	// 
	// 	player = FindObjectOfType<PlayerWalk>().gameObject;
	// 	pickupText = transform.Find( "PickupText" ).gameObject;
	// 	cam = Camera.main;
	// }
	// 
	// void Update()
	// {
	// 	var diff = player.transform.position - transform.position;
	// 	if( diff.sqrMagnitude < pickupDist * pickupDist )
	// 	{
	// 		pickupText.SetActive( true );
	// 
	// 		var rot = pickupText.transform.eulerAngles;
	// 		var camDiff = cam.transform.position - transform.position;
	// 		rot.y = Mathf.Atan2( camDiff.x,camDiff.z ) * Mathf.Rad2Deg + 180.0f;
	// 		pickupText.transform.eulerAngles = rot;
	// 
	// 		if( SpiffyInput.CheckAxis( "Interact" ) )
	// 		{
	// 			// player.GetComponent<WeaponHolder>().ReplaceWeapon( pickupPrefab );
	// 			player.GetComponent<PlayerInventory>().AddItem( pickupPrefab );
	// 			Destroy( gameObject );
	// 		}
	// 	}
	// 	else pickupText.SetActive( false );
	// }

	protected override void Start()
	{
		base.Start();

		Assert.IsTrue( pickupPrefab != null );
		var pickup = Instantiate( pickupPrefab.GetPrefab().transform.GetChild( 0 ).gameObject,transform );
		// pickup.transform.Rotate( Vector3.forward,90.0f );
		pickup.transform.position = transform.Find( "WeaponHoldPos" ).position;
	}

	protected override void Interact()
	{
		player.GetComponent<PlayerInventory>().GetInv().AddItem( pickupPrefab );
		Destroy( gameObject );
	}

	// [SerializeField] float pickupDist = 4.0f;
	[SerializeField] LoadableItem pickupPrefab = null;

	// GameObject pickup;
	// GameObject player;
	// GameObject pickupText;
	// Camera cam;
}
