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

		respawnParticles = transform.Find( "Main Camera" ).Find( "RespawnParticles" ).GetComponent<ParticleSystem>();
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
			gameObject.layer = LayerMask.NameToLayer( "NoCollide" );
			cam.transform.rotation = safeRot;
			// moveScr.enabled = true;
			StartCoroutine( RespawnParts() );
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
			respawnParticles.Emit( savePartCount.Rand() );
			Destroy( coll.gameObject );
		}
	}

	IEnumerator RespawnParts()
	{
		yield return( new WaitForSeconds( 0.1f ) );

		respawnParticles.Emit( respawnPartCount.Rand() );
	}

	CharacterController charCtrl;
	Camera cam;
	ParticleSystem respawnParticles;

	[SerializeField] float respawnY = -5.0f;
	[SerializeField] RangeI savePartCount = new RangeI( 15,22 );
	[SerializeField] RangeI respawnPartCount = new RangeI( 15,22 );

	Vector3 safeSpot;
	Quaternion safeRot;
}
