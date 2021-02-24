using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageablePlayer
	:
	Damageable
{
	protected override void Oof()
	{
		// base.Oof();
		SceneManager.LoadScene( "HubScene" );
	}
}
