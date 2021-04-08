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
		SceneManager.LoadScene( "ForestDungeon" );
	}

	public void Tutorial()
	{
		SceneManager.LoadScene( "TutorialScene" );
	}

	public void ResetProgress()
	{
		Upgrade.reset = true;
		XPUI.ResetAll();

		Play();
	}

	public void Continue()
	{
		SceneManager.LoadScene( PlayerPrefs.GetInt( "save_scene",1 ) );
	}

	public void NewGame()
	{
		SceneManager.LoadScene( 1 );
	}

	public void Quit()
	{
		Application.Quit();
	}
}
