using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase
	:
	MonoBehaviour
{
	void Start()
	{
		body = GetComponent<Rigidbody>();
		cam = Camera.main;
		animCtrl = GetComponent<Animator>();
	}

	void Update()
	{
		if( PauseMenu.IsOpen() ) return;

		var move = new Vector3(
			Input.GetAxis( "Horizontal" ),
			0.0f,
			Input.GetAxis( "Vertical" ) );
		move.Normalize();
		var ang = cam.transform.eulerAngles.y * Mathf.Deg2Rad - Mathf.PI / 2.0f;

		var xMove = Mathf.Cos( ang ) * move.z + Mathf.Sin( ang + Mathf.PI ) * move.x;
		var yMove = -Mathf.Sin( ang ) * move.z + Mathf.Cos( ang + Mathf.PI ) * move.x;

		if( Mathf.Abs( xMove ) > 0.0f || Mathf.Abs( yMove ) > 0.0f )
		{
			var rot = transform.eulerAngles;
			rot.y = Mathf.Atan2( xMove,yMove ) * Mathf.Rad2Deg;
			rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
			transform.eulerAngles = rot;
		}

		body.AddForce( new Vector3( xMove,0.0f,yMove ) * accel );
		if( body.velocity.sqrMagnitude > maxSpeed * maxSpeed )
		{
			body.velocity = body.velocity.normalized * maxSpeed;
		}

		animCtrl.SetBool( "attack",false );
		if( SpiffyInput.CheckFree( "Fire1" ) )
		{
			var forward = cam.transform.forward;
			forward.y = 0.0f;
			transform.forward = forward;

			animCtrl.SetBool( "attack",true );
		}
	}

	Rigidbody body;
	Camera cam;
	Animator animCtrl;

	[SerializeField] float accel = 10.0f;
	[SerializeField] float maxSpeed = 10.0f;
	[SerializeField] float rotSpeed = 4.0f;
}
