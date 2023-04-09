using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPickup
	:
	MonoBehaviour
{
	void Start()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
		cam = Camera.main;
		
		pickupText = Instantiate( ResLoader.Load( "Prefabs/HoverText" ) )
			.GetComponentInChildren<TextMesh>();
		pickupText.text = interactMsg;
		pickupText.gameObject.SetActive( false );

		rayMask = LayerMask.GetMask( "ToolPickup" );
		pickupText.transform.position = transform.position + Vector3.up * heightOffset;
	}

	void Update()
	{
		bool oldActive = textActive;
		textActive = false;

		var dist = player.transform.position - transform.position;
		if( dist.sqrMagnitude < pickupDist * pickupDist )
		{
			var ray = new Ray( cam.transform.position,cam.transform.forward );
			RaycastHit hit;

			if( Physics.Raycast( ray,out hit,pickupDist * 2,rayMask ) )
			{
				if( hit.transform.gameObject == gameObject )
				{
					textActive = true;
					
					if( Input.GetAxis( interactKey ) > 0.0f )
					{
						// todo: equip tool
					}
				}
			}
		}

		if( textActive != oldActive ) pickupText.gameObject.SetActive( textActive );
	}

	GameObject player;
	Camera cam;
	TextMesh pickupText;

	[SerializeField] float pickupDist = 3.0f;
	[SerializeField] float heightOffset = 1.0f;
	[SerializeField] string interactMsg = "[E] Pickup Shotgun";
	[SerializeField] string interactKey = "Interact1";

	LayerMask rayMask;
	bool textActive = false;
}