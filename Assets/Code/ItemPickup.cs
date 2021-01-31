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
		player = FindObjectOfType<PlayerWalk>().gameObject;
		pickupText = Instantiate( Resources.Load<GameObject>( "Prefabs/HoverText" ) );
		pickupText.transform.position = transform.position;
		pickupText.GetComponentInChildren<TextMesh>().text = "E";
		pickupText.SetActive( false );
		cam = Camera.main;

		prefab = Resources.Load<GameObject>( prefabSrc );
		Assert.IsNotNull( prefab );
	}

	void Update()
	{
		var diff = player.transform.position - transform.position;
		if( diff.sqrMagnitude < pickupDist * pickupDist )
		{
			pickupText.SetActive( true );
			pickupText.transform.position = transform.position;

			var rot = pickupText.transform.eulerAngles;
			var camDiff = cam.transform.position - transform.position;
			rot.y = Mathf.Atan2( camDiff.x,camDiff.z ) * Mathf.Rad2Deg + 180.0f;
			pickupText.transform.eulerAngles = rot;

			if( Input.GetAxis( "Interact" ) > 0.0f )
			{
				player.GetComponent<PlayerInventory>().AddItem( prefab );
				Destroy( pickupText );
				Destroy( gameObject );
			}
		}
		else pickupText.SetActive( false );
	}

	[SerializeField] string prefabSrc = "";
	GameObject prefab = null;
	[SerializeField] float pickupDist = 4.0f;

	GameObject pickup;
	GameObject player;
	GameObject pickupText;
	Camera cam;
}
