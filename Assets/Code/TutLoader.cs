using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutLoader
	:
	MonoBehaviour
{
	void Start()
	{
		if( SceneManager.GetActiveScene().name == "DungeonScene" )
		{
			PlayerPrefs.SetInt( "completedtut",1 );
		}

		if( !loaded )
		{
			loaded = true;
			bool completedTutorial = PlayerPrefs.GetInt( "completedtut",0 ) > 0;
			if( !completedTutorial ) SceneManager.LoadScene( "TutorialScene" );
		}
	}

	static bool loaded = false;
}
