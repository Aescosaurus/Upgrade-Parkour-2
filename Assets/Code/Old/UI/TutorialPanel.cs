using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel
	:
	MonoBehaviour
{
	void Start()
	{
		img = GetComponent<Image>();

		for( int i = 0; i < transform.childCount; ++i )
		{
			children.Add( transform.GetChild( i ).gameObject );
		}

		ToggleOpen( false );
	}

	void Update()
	{
		if( SpiffyInput.CheckAxis( "Inventory" ) )
		{
			ToggleOpen( !open );
		}
	}

	public void ToggleOpen( bool open )
	{
		this.open = open;

		img.enabled = open;

		foreach( var child in children )
		{
			child.SetActive( open );
		}
	}

	List<GameObject> children = new List<GameObject>();
	Image img;

	bool open = false;
}
