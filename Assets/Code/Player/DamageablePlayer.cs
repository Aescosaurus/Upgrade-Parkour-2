using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class DamageablePlayer
	:
	Damageable
{
	protected override void Start()
	{
		base.Start();

		Assert.IsTrue( GetTeam() == 1 );
	}

	protected override void Oof()
	{
		if( !oofed )
		{
			oofed = true;
			// base.Oof();

			// PauseMenu.SetOpen( true );
			Destroy( GetComponent<PlayerBase>() );

			StartCoroutine( ReloadScene() );
		}
	}

	IEnumerator ReloadScene()
	{
		yield return( new WaitForSeconds( pauseDur ) );

		SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
	}

	[SerializeField] float pauseDur = 0.5f;
}
