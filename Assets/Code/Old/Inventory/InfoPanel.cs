using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel
	:
	MonoBehaviour
{
	void Start()
	{
		img = GetComponent<Image>();
		title = transform.Find( "Title" ).GetComponent<Text>();
		desc = transform.Find( "Desc" ).GetComponent<Text>();

		ClosePanel();
	}

	public void OpenPanel( string title,string desc )
	{
		img.enabled = true;
		this.title.text = title;
		this.desc.text = desc;
	}

	public void ClosePanel()
	{
		if( img != null )
		{
			img.enabled = false;
			title.text = "";
			desc.text = "";
		}
	}

	Image img;
	Text title;
	Text desc;
}
