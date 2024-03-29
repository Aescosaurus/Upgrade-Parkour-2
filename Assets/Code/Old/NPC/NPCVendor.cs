﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NPCVendor
	:
	NPCDialog
{
	protected override void Start()
	{
		base.Start();

		Assert.IsNotNull( uiPrefab );

		invHand = FindObjectOfType<InventoryHandler>();
		miscPanel = GameObject.Find( "MiscPanel" );

		var animCtrl = GetComponent<Animator>();
		if( animCtrl != null ) animCtrl.speed = animSpeed;
	}

	protected override void Update()
	{
		base.Update();

		// if( open && SpiffyInput.CheckAxis( "Inventory" ) )
		// {
		// 	CloseUI();
		// }
	}

	protected override void Interact()
	{
		if( !open )
		{
			invHand.ToggleOpen( false );
			base.Interact();
		}

		// if( curLine > lines.Count - 1 )
		// {
		// 	OpenUI();
		// }
	}

	protected virtual void OpenUI()
	{
		invHand.ToggleOpen( true );

		curUI = Instantiate( uiPrefab,miscPanel.transform );
		curUI.GetComponent<VendorUIBase>().SetVendor( gameObject );
		open = true;
	}

	// void CloseUI()
	// {
	// 	// todo drop items left in inv slots in ui
	// 
	// 	Destroy( curUI );
	// 	invHand.ToggleOpen( false );
	// 	// open = false;
	// 	SetText( "[E]" );
	// }
	public virtual void CloseUI()
	{
		open = false;
		SetText( "[E]" );
	}

	protected override void EndDialog()
	{
		base.EndDialog();
		
		SetText( "" );
		OpenUI();
	}

	protected override void UnInteract()
	{
		base.UnInteract();

		if( open )
		{
			// CloseUI();
			// curUI?.GetComponent<VendorUIBase>()?.CloseUI();
			// if( miscPanel.transform.childCount > 0 ) Destroy( miscPanel.transform.GetChild( 0 ).gameObject );
			// 
			// infoPanel.ClosePanel();
			invHand.ToggleOpen( false );
		}
	}

	[SerializeField] GameObject uiPrefab = null;
	protected GameObject curUI = null;

	[SerializeField] float animSpeed = 1.0f;

	InventoryHandler invHand;
	GameObject miscPanel;
	bool open = false;
}
