using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalStairs
	:
	Portal
{
	protected override void Interact()
	{
		// We no want base upsetting curfloor value; increment it and reload scene instead.
		// base.Interact();

		PlayerPrefs.SetInt( "curfloor",PlayerPrefs.GetInt( "curfloor",0 ) + 1 );

		SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
	}
}
