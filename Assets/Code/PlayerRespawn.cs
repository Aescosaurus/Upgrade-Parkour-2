using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn
	:
	MonoBehaviour
{
	void Update()
	{
		if( transform.position.y < respawnY )
		{
			SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
		}
	}

	[SerializeField] float respawnY = -5.0f;
}
