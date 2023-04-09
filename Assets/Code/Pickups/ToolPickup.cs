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
				Destroy( interactText.gameObject );
				Destroy( gameObject );
			}
		}
	}

	PlayerMove2 playerMove;

	[SerializeField] PlayerMove2.Equip equip = PlayerMove2.Equip.None;
	[SerializeField] bool destroyAfterPickup = false;
}