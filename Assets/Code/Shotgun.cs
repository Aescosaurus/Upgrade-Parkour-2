using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun
	:
	MonoBehaviour
{
	void Start()
	{
		cam = Camera.main;

		var player = transform.root.gameObject;
		playerMoveScr = player.GetComponent<PlayerMove>();
		charCtrl = player.GetComponent<CharacterController>();

		shotMask = LayerMask.GetMask( "Player" );
	}

	void Update()
	{
		refire.Update( Time.deltaTime );
		if( SpiffyInput.CheckAxis( "Fire1" ) && canFire && refire.IsDone() )
		{
			var knockbackDir = -cam.transform.forward;
			RaycastHit hit;
			if( Physics.Raycast( cam.transform.position,cam.transform.forward,out hit,shotMask ) )
			{
				playerMoveScr.ApplyForceMove( knockbackDir.normalized * Mathf.Min(
					knockbackForce * ( 1.0f / hit.distance ),maxForce ) );

				canFire = false;
				refire.Reset();
			}
		}

		if( charCtrl.isGrounded )
		{
			canFire = true;
		}
	}

	Camera cam;
	PlayerMove playerMoveScr;
	LayerMask shotMask;
	CharacterController charCtrl;

	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] float maxForce = 500.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );

	bool canFire = true;
}
