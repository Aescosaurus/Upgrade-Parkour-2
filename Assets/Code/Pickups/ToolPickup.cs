using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPickup
	:
	HoverInteract
{
	protected override void Start()
	{
		base.Start();

		playerMove = player.GetComponent<PlayerMove2>();

		pickupText = Instantiate( ResLoader.Load( "Prefabs/HoverText" ) )
			.GetComponentInChildren<TextMesh>();
		pickupText.text = interactMsg;
		pickupText.gameObject.SetActive( false );

		pickupText.transform.position = transform.position + Vector3.up * heightOffset;
	}

	protected override void OnInteract()
	{
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

	protected override void OnInteractToggle( bool on )
	{
		pickupText.gameObject.SetActive( on );
	}

	PlayerMove2 playerMove;
	TextMesh pickupText;

	[SerializeField] float heightOffset = 1.0f;
	[SerializeField] string interactMsg = "[E]/[Q] Pickup";
	[SerializeField] PlayerMove2.Equip equip = PlayerMove2.Equip.None;
	[SerializeField] bool destroyAfterPickup = false;
}