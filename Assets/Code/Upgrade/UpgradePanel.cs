using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel
	:
	MonoBehaviour
{
	void Start()
	{
		img = GetComponent<Image>();
		origOpacity = img.color.a;

		for( int i = 0; i < transform.childCount; ++i )
		{
			children.Add( transform.GetChild( i ).gameObject );
		}

		ToggleOpen( false );
	}

	void Update()
	{
		if( SpiffyInput.CheckAxis( "Inventory",true ) ) ToggleOpen( !open );
	}

	void ToggleOpen( bool open )
	{
		this.open = open;

		// var c = img.color;
		// c.a = open ? origOpacity : 0.0f;
		// img.color = c;
		img.enabled = open;

		foreach( var child in children )
		{
			child.SetActive( open );
		}

		PauseMenu.SetOpen( open );

		Cursor.visible = open;
		Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
	}

	Image img;
	float origOpacity;
	List<GameObject> children = new List<GameObject>();

	bool open = false;
}