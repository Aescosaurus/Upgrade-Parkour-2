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
		playerMove = player.GetComponent<PlayerMove2>();
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
					
					int pickupSlot = -1;
					
					if( SpiffyInput.CheckAxis( "Interact1" ) ) pickupSlot = 2;
					else if( SpiffyInput.CheckAxis( "Interact2" ) ) pickupSlot = 1;
					
					if( pickupSlot > 0 )
					{
						playerMove.EquipItem( equip,pickupSlot );
						ToolManager.EquipItem( equip,pickupSlot );

						if( destroyAfterPickup )
						{
							Destroy( pickupText.gameObject );
							Destroy( gameObject );
						}
					}
				}
			}
		}

		if( textActive != oldActive ) pickupText.gameObject.SetActive( textActive );
	}

	GameObject player;
	PlayerMove2 playerMove;
	Camera cam;
	TextMesh pickupText;

	[SerializeField] float pickupDist = 3.0f;
	[SerializeField] float heightOffset = 1.0f;
	[SerializeField] string interactMsg = "[E]/[Q] Pickup";
	[SerializeField] PlayerMove2.Equip equip = PlayerMove2.Equip.None;
	[SerializeField] bool destroyAfterPickup = false;

	LayerMask rayMask;
	bool textActive = false;
}