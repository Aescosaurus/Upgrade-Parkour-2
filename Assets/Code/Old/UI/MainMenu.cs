using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu
	:
	MonoBehaviour
{
	void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		transform.Find( "ContinueButton" ).GetComponent<Button>().interactable =
			( PlayerPrefs.GetInt( "save_scene",0 ) > 0 );
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
		// todo reset player save itenz

		PlayerPrefs.SetInt( "save_scene",1 );

		Continue();
	}

	public void Quit()
	{
		Application.Quit();
	}
}
