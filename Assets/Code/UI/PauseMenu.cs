using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu
	:
	MonoBehaviour
{
	void Start()
	{
		img = GetComponent<Image>();
		initAlpha = img.color.a;
		CloseMenu();
	}

	void Update()
	{
		if( SpiffyInput.CheckAxis( "Menu",true ) )
		{
			ToggleOpen( !open );
		}
	}

	public void CloseMenu()
	{
		ToggleOpen( false );
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene( "MainMenuScene" );
	}

	public void ToggleOpen( bool open )
	{
		// PauseMenu.open = open;
		
		// var c = img.color;
		// c.a = open ? initAlpha : 0.0f;
		// img.color = c;
		img.enabled = open;

		var nButtons = transform.childCount;
		for( int i = 0; i < nButtons; ++i )
		{
			transform.GetChild( i ).gameObject.SetActive( open );
		}

		SetOpen( open );
	}

	public static void SetOpen( bool isOpen )
	{
		open = isOpen;

		Time.timeScale = isOpen ? 0.0f : 1.0f;

		Cursor.visible = isOpen;
		Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
	}

	public static bool IsOpen()
	{
		return( open );
	}

	Image img;
	static bool open = false;

	float initAlpha;
}
