using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn
	:
	MonoBehaviour
{
	void Start()
	{
		charCtrl = GetComponent<CharacterController>();
		cam = Camera.main;

		safeSpot = transform.position;
		safeRot = cam.transform.rotation;
	}

	void Update()
	{
		if( transform.position.y < respawnY )
		{
			// SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );

			var moveScr = GetComponent<PlayerMove>();
			// moveScr.enabled = false;
			// transform.position = safeSpot;
			moveScr.Reset( safeSpot );
			cam.transform.rotation = safeRot;
			// moveScr.enabled = true;
		}

		// if( charCtrl.isGrounded )
		// {
		// 	safeSpot = transform.position;
		// }
	}

	void OnTriggerEnter( Collider coll )
	{
		if( coll.tag == "Respawn" )
		{
			safeSpot = transform.position;
			safeRot = cam.transform.rotation;
			Destroy( coll.gameObject );
		}
	}

	CharacterController charCtrl;
	Camera cam;

	[SerializeField] float respawnY = -5.0f;

	Vector3 safeSpot;
	Quaternion safeRot;
}
