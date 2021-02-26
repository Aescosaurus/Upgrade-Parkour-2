using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu
	:
	MonoBehaviour
{
	void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void Play()
	{
		SceneManager.LoadScene( "HubScene" );
	}

	public void Tutorial()
	{
		SceneManager.LoadScene( "TutorialScene" );
	}

	public void Quit()
	{
		Application.Quit();
	}
}
