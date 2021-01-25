using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WeaponPickup
	:
	MonoBehaviour
{
	void Start()
	{
		Assert.IsTrue( pickupPrefab != null );
		pickup = Instantiate( pickupPrefab.transform.GetChild( 0 ).gameObject,transform );
		// pickup.transform.Rotate( Vector3.forward,90.0f );
		pickup.transform.position = transform.Find( "WeaponHoldPos" ).position;

		player = FindObjectOfType<PlayerWalk>().gameObject;
		pickupText = transform.Find( "PickupText" ).gameObject;
		cam = Camera.main;
	}

	void Update()
	{
		var diff = player.transform.position - transform.position;
		if( diff.sqrMagnitude < pickupDist * pickupDist )
		{
			pickupText.SetActive( true );

			var rot = pickupText.transform.eulerAngles;
			rot.y = Mathf.Atan2( diff.x,diff.z ) * Mathf.Rad2Deg + 180.0f;
			pickupText.transform.eulerAngles = rot;

			if( Input.GetAxis( "Interact" ) > 0.0f )
			{
				player.GetComponent<WeaponHolder>().ReplaceWeapon( pickupPrefab );
				Destroy( gameObject );
			}
		}
		else pickupText.SetActive( false );
	}

	// make sure to dupe prefab again for pickup

	[SerializeField] float pickupDist = 4.0f;
	[SerializeField] GameObject pickupPrefab = null;

	GameObject pickup;
	GameObject player;
	GameObject pickupText;
	Camera cam;
}
