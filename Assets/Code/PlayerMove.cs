using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove
	:
	MonoBehaviour
{
	void Start()
	{
		body = GetComponent<Rigidbody>();
		cam = Camera.main;
		// animCtrl = GetComponent<Animator>();
		// coll = GetComponent<Collider>();
		charCtrl = GetComponent<CharacterController>();

		transform.Find( "Model" ).gameObject.SetActive( false );

		PlayerPrefs.SetInt( "save_scene",SceneManager.GetActiveScene().buildIndex );

		Instantiate( ResLoader.Load( "Prefabs/Canvas" ) );
		Instantiate( ResLoader.Load( "Prefabs/EventSys" ) );

		// todo load save items

		if( hasShotgun )
		{
			Instantiate( ResLoader.Load( "Prefabs/Shotgun" ),transform.Find( "Main Camera" ).Find( "WepHoldSpot" ) );
			stopForceMove = true;
		}
	}

	void /*Fixed*/Update()
	{
		// if( PauseMenu.IsOpen() ) return;

		if( CanJump() )
		{
			canJump = true;
			jumpLeniency.Reset();
			yVel = 0.0f;
		}
		else if( jumpLeniency.Update( Time.deltaTime ) )
		{
			canJump = false;
		}

		var vertAxis = SpiffyInput.GetAxis( "Vertical" );
		var forwardAxisPower = vertAxis;

		// if( canJump ) forwardAxisPower = vertAxis;

		var move = new Vector3(
			SpiffyInput.GetAxis( "Horizontal" ),
			0.0f,
			forwardAxisPower
		);
		// move.Normalize();
		var ang = cam.transform.eulerAngles.y * Mathf.Deg2Rad - Mathf.PI / 2.0f;

		var xMove = Mathf.Cos( ang ) * move.z + Mathf.Sin( ang + Mathf.PI ) * move.x;
		var yMove = -Mathf.Sin( ang ) * move.z + Mathf.Cos( ang + Mathf.PI ) * move.x;

		vel.x += xMove * moveSpeed * Time.deltaTime;
		vel.y += yMove * moveSpeed * Time.deltaTime;

		// xMove *= bhMod * bhStrafeMod;
		// yMove *= bhMod;

		// if( Mathf.Abs( xMove ) > 0.0f || Mathf.Abs( yMove ) > 0.0f )
		// {
		// 	var rot = transform.eulerAngles;
		// 	rot.y = Mathf.Atan2( xMove,yMove ) * Mathf.Rad2Deg;
		// 	rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
		// 	// transform.eulerAngles = rot;
		// 
		// 	// animCtrl.SetBool( "walk",true );
		// }
		// else
		// {
		// 	// animCtrl.SetBool( "walk",false );
		// }

		if( SpiffyInput.GetAxis( "Jump" ) > 0.0f )
		{
			if( !jumping && canJump )
			{
				jumping = true;
			}
		}
		else if( variableJump )
		{
			if( jumping && minJump.Update( Time.deltaTime ) )
			{
				StopJumping();
			}
		}

		if( jumping )
		{
			yVel = jumpPower;

			if( jumpTimer.Update( Time.deltaTime ) )
			{
				StopJumping();
			}
		}
		else
		{
			yVel -= gravAcc * Time.deltaTime;
		}

		if( vel.magnitude > maxSpeed ) vel = vel.normalized * maxSpeed;

		if( resetPos == Vector3.zero )
		{
			charCtrl.Move( ( new Vector3( vel.x,yVel,vel.y ) * moveSpeed + forceMove ) * Time.deltaTime );
		}
		else
		{
			var diff = resetPos - transform.position;
			charCtrl.Move( diff );
			resetPos = Vector3.zero;
		}

		// animCtrl.SetBool( "jump",yVel > 0.0f );
		// animCtrl.SetBool( "jump",!canJump );

		if( charCtrl.isGrounded )
		{
			if( stopForceMove || !SpiffyInput.CheckFree( "Sprint" ) )
			{
				forceMove.Set( 0.0f,0.0f,0.0f );
			}
		}

		if( canSprint )
		{
			if( SpiffyInput.CheckFree( "Sprint" ) )
			{
				if( forceMove.sqrMagnitude < maxSprintSpd * maxSprintSpd )
				{
					forceMove += cam.transform.forward * sprintAccel * Time.deltaTime;
					// forceMove.x += xMove * sprintAccel * Time.deltaTime;
					// forceMove.z += yMove * sprintAccel * Time.deltaTime;
				}
			}
		}
	}

	void FixedUpdate()
	{
		vel *= decel;
		forceMove *= forceDecay;
	}

	void OnTriggerEnter( Collider coll )
	{
		if( coll.gameObject != gameObject && stopForceMove )
		{
			forceMove *= forcePenalty;
		}
	}

	void StopJumping()
	{
		jumping = false;
		jumpTimer.Reset();
		minJump.Reset();
		yVel /= 2.0f;
	}

	public void Reset( Vector3 resetPos )
	{
		yVel = 0.0f;
		vel.Set( 0.0f,0.0f );
		canJump = false;

		this.resetPos = resetPos;
	}

	public void ApplyForceMove( Vector3 move )
	{
		forceMove += move;
	}

	bool CanJump()
	{
		return( charCtrl.isGrounded );
	}

	Rigidbody body;
	Camera cam;
	// Animator animCtrl;
	// Collider coll;
	CharacterController charCtrl;

	[SerializeField] float moveSpeed = 10.0f;

	[SerializeField] float jumpPower = 3.0f;
	[SerializeField] Timer jumpTimer = new Timer( 2.0f );
	[SerializeField] Timer minJump = new Timer( 0.5f );
	[SerializeField] bool variableJump = true;

	bool jumping = false;

	[SerializeField] float gravAcc = 0.3f;

	float yVel = 0.0f;
	Vector2 vel = Vector2.zero;

	[SerializeField] float decel = 0.9f;
	[SerializeField] float maxSpeed = 1.0f;

	[SerializeField] Timer jumpLeniency = new Timer( 0.2f );
	bool canJump = false;

	Vector3 resetPos = Vector3.zero;

	[SerializeField] bool hasShotgun = false;
	[SerializeField] bool canSprint = false;

	bool stopForceMove = false;

	Vector3 forceMove = Vector3.zero;
	[SerializeField] float forceDecay = 0.9f;
	[SerializeField] float forcePenalty = 0.5f;
	[SerializeField] float sprintAccel = 10.0f;
	[SerializeField] float maxSprintSpd = 30.0f;
}
