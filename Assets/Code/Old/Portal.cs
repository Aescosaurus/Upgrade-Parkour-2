using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal
	:
	InteractiveBase
{
	protected override void Start()
	{
		base.Start();

		SetText( "[E] " + displayText );
	}

	protected override void Interact()
	{
		// todo check player stats before allowing entry
		PlayerPrefs.SetInt( "curfloor",0 );

		SceneManager.LoadScene( worldTarget );
	}

	[SerializeField] string worldTarget = "";
	[SerializeField] string displayText = "";
}
