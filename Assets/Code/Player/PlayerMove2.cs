﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerMove2
	:
	MonoBehaviour
{
	public enum Equip
	{
		None,
		GrapplingHook,
		Shotgun,
		C4,
		FireworkRocket,
		ZipHook
	}

	void Start()
	{
		cam = Camera.main;
		charCtrl = GetComponent<CharacterController>();
		audSrc = transform.Find( "SFX" ).GetComponent<AudioSource>();

		// PlayerPrefs.SetInt( "save_scene",SceneManager.GetActiveScene().buildIndex );

		Instantiate( ResLoader.Load( "Prefabs/UI/Canvas" ) );
		Instantiate( ResLoader.Load( "Prefabs/UI/EventSys" ) );

		if( forceEquipItems )
		{
			EquipItem( item1,1 );
			EquipItem( item2,2 );
		}
		else
		{
			EquipItem( ToolManager.GetHandEquip( 1 ),1 );
			EquipItem( ToolManager.GetHandEquip( 2 ),2 );
		}

		jumpSound = Resources.Load<AudioClip>( "Audio/Jump" );
		landSound = Resources.Load<AudioClip>( "Audio/Land" );

		transform.Find( "Main Camera" ).Find( "GrappleParticles" ).gameObject.SetActive( false );
	}

	void /*Fixed*/Update()
	{
		move = new Vector3(
			SpiffyInput.GetAxis( "Horizontal" ),
			0.0f,
			SpiffyInput.GetAxis( "Vertical" )
		);
	}

	void FixedUpdate()
	{
		// if( PauseMenu.IsOpen() ) return;

		if( CanJump() )
		{
			canJump = true;
			jumpLeniency.Reset();
			yVel = 0.0f;
		}
		else if( jumpLeniency.Update( Time.fixedDeltaTime ) )
		{
			canJump = false;
		}

		// if( canJump ) forwardAxisPower = vertAxis;
		// move.Normalize();
		var ang = cam.transform.eulerAngles.y * Mathf.Deg2Rad - Mathf.PI / 2.0f;

		var xMove = Mathf.Cos( ang ) * move.z + Mathf.Sin( ang + Mathf.PI ) * move.x;
		var yMove = -Mathf.Sin( ang ) * move.z + Mathf.Cos( ang + Mathf.PI ) * move.x;

		vel.x += xMove * moveSpeed * Time.fixedDeltaTime;
		vel.y += yMove * moveSpeed * Time.fixedDeltaTime;

		// xMove *= bhMod * bhStrafeMod;
		// yMove *= bhMod;

		// if( Mathf.Abs( xMove ) > 0.0f || Mathf.Abs( yMove ) > 0.0f )
		// {
		// 	var rot = transform.eulerAngles;
		// 	rot.y = Mathf.Atan2( xMove,yMove ) * Mathf.Rad2Deg;
		// 	rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.fixedDeltaTime );
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

				if( yVel < 0.2f ) audSrc.PlayOneShot( jumpSound );
			}
		}
		else if( variableJump )
		{
			if( jumping && minJump.Update( Time.fixedDeltaTime ) )
			{
				StopJumping();
			}
		}

		if( jumping )
		{
			yVel = jumpPower;

			if( jumpTimer.Update( Time.fixedDeltaTime ) )
			{
				StopJumping();
			}
		}
		else
		{
			yVel -= gravAcc * Time.fixedDeltaTime;
		}

		if( vel.magnitude > maxSpeed ) vel = vel.normalized * maxSpeed;

		if( !resetPending )
		{
			charCtrl.Move( GetVel() * Time.fixedDeltaTime );

			if( footstepTimer.Update( Time.fixedDeltaTime ) && charCtrl.isGrounded && move.sqrMagnitude > 0.2f )
			{
				footstepTimer.Reset();
				audSrc.PlayOneShot( footstepSounds[Random.Range( 0,footstepSounds.Count )] );
			}
		}
		else
		{
			var diff = resetPos - transform.position;
			charCtrl.Move( diff );
			resetPos = Vector3.zero;
			resetPending = false;
			gameObject.layer = LayerMask.NameToLayer( "Player" );
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

		vel *= decel;
		forceMove *= forceDecay;
	}

	void OnTriggerEnter( Collider coll )
	{
		if( coll.gameObject != gameObject )
		{
			if( stopForceMove ) forceMove *= forcePenalty;
			audSrc.PlayOneShot( landSound );
		}
	}

	void StopJumping()
	{
		jumping = false;
		jumpTimer.Reset();
		minJump.Reset();
		yVel *= jumpEndPenalty;
	}

	public void Reset( Vector3 resetSpot )
	{
		yVel = 0.0f;
		vel.Set( 0.0f,0.0f );
		forceMove.Set( 0.0f,0.0f,0.0f );
		canJump = false;

		this.resetPos = resetSpot;
		resetPending = true;
	}

	public void ApplyForceMove( Vector3 move )
	{
		forceMove += move;
	}

	// Force move but sets y dir instead of adding.
	public void ForceMoveCancel( Vector3 move )
	{
		forceMove.y = 0.0f;
		ApplyForceMove( move );
	}

	public void ResetGrav()
	{
		yVel = 0.0f;
	}

	public void EquipItem( Equip item,int hand )
	{
		Assert.IsTrue( hand == 1 || hand == 2 );

		var prefab = ResLoader.Load( "Prefabs/Equips/" + item.ToString() );

		if( prefab != null )
		{
			if( curEquips[hand - 1] != null ) Destroy( curEquips[hand - 1].gameObject );

			var curItem = Instantiate( prefab,cam.transform.Find( "WepHoldSpot" + hand.ToString() ) );
			var curTool = curItem.GetComponent<ToolBase>();
			curTool.SetInputKey( "Fire" + hand.ToString() );

			curEquips[hand - 1] = curTool;
		}
	}

	public void ReloadEquips()
	{
		foreach( var equip in curEquips ) equip.Reload();
	}

	bool CanJump()
	{
		return( charCtrl.isGrounded );
	}

	public Vector3 GetVel()
	{
		return( new Vector3( vel.x,yVel,vel.y ) * moveSpeed + forceMove );
	}

	// Rigidbody body;
	Camera cam;
	// Animator animCtrl;
	// Collider coll;
	CharacterController charCtrl;
	AudioSource audSrc;

	[Header( "Movement" )]
	[SerializeField] float moveSpeed = 10.0f;
	[SerializeField] float decel = 0.9f;
	[SerializeField] float maxSpeed = 1.0f;

	[Header( "Jumping" )]
	[SerializeField] float jumpPower = 3.0f;
	[SerializeField] Timer jumpTimer = new Timer( 2.0f );
	[SerializeField] Timer minJump = new Timer( 0.5f );
	[SerializeField] bool variableJump = true;
	[SerializeField] float jumpEndPenalty = 0.5f;

	bool jumping = false;

	[SerializeField] float gravAcc = 0.3f;

	float yVel = 0.0f;
	Vector2 vel = Vector2.zero;

	[SerializeField] Timer jumpLeniency = new Timer( 0.2f );
	bool canJump = false;

	Vector3 resetPos = Vector3.zero;
	bool resetPending = false;

	bool stopForceMove = false;

	[Header( "Force Stuff" )]
	Vector3 forceMove = Vector3.zero;
	[SerializeField] float forceDecay = 0.9f;
	[SerializeField] float forcePenalty = 0.5f;

	[Header( "Audio" )]
	[SerializeField] List<AudioClip> footstepSounds = new List<AudioClip>();
	[SerializeField] Timer footstepTimer = new Timer( 0.2f );
	AudioClip jumpSound;
	AudioClip landSound;

	Vector3 move = Vector3.zero;

	[Header( "Items" )]
	[SerializeField] Equip item1 = Equip.None;
	[SerializeField] Equip item2 = Equip.None;
	[Tooltip( "true = don't load items from save" )]
	[SerializeField] bool forceEquipItems = false;

	ToolBase[] curEquips = new ToolBase[2];
}
