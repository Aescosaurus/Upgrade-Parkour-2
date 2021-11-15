using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4
	:
	ToolBase
{
	void Start()
	{
		cam = Camera.main;

		var player = transform.root.gameObject;
		playerMoveScr = player.GetComponent<PlayerMove2>();
		charCtrl = player.GetComponent<CharacterController>();
		// trailPrefab = ResLoader.Load( "Prefabs/GrapplingHookTrail" );
		// hook = transform.Find( "GrapplingHook" ).gameObject;
		// particlePrefab = ResLoader.Load( "Prefabs/GrappleParticles" );
		playerParticles = player.transform.Find( "Main Camera" ).Find( "GrappleParticles" ).gameObject;

		// shotMask = LayerMask.GetMask( "Default" );

		FireReset();
	}

	void Update()
	{
		refire.Update( Time.deltaTime );
		if( SpiffyInput.CheckFree( inputKey ) )
		{
			if( c4Obj == null && refire.IsDone() )
			{
				c4Obj = Instantiate( c4Prefab/*,playerMoveScr.transform*/ );
				c4Obj.transform.position = cam.transform.position + cam.transform.forward * throwStartDist;
				var rb = c4Obj.GetComponent<Rigidbody>();
				rb.velocity = playerMoveScr.GetVel();
				rb.AddForce( cam.transform.forward * throwForce,ForceMode.Impulse );
			}
		}
		else
		{
			if( c4Obj != null )
			{
				// explode
				var dir = c4Obj.transform.position - playerMoveScr.transform.position;
				float force = Mathf.Pow( explodeRange,2 ) - dir.sqrMagnitude;
				if( force > 0.0f ) playerMoveScr.ApplyForceMove( -dir.normalized * force * explodeForce );
				Destroy( c4Obj );
				c4Obj = null;
			}
		}
	}

	void FireReset()
	{
		refire.Reset();
		pullDuration.Reset();

		canFire = false;
		hitObj = null;
		hitOffset = Vector3.zero;

		playerMoveScr.ResetGrav();

		Destroy( curTrail?.gameObject );
		curTrail = null;
		Destroy( hookParticles );
		hookParticles = null;
		playerParticles.SetActive( false );
	}

	Camera cam;
	PlayerMove2 playerMoveScr;
	LayerMask shotMask;
	CharacterController charCtrl;
	GameObject trailPrefab;
	GameObject particlePrefab;
	GameObject playerParticles;
	Transform hitObj = null;
	Vector3 hitOffset = Vector3.zero;
	LineRenderer curTrail = null;
	GameObject hook = null;
	GameObject hookParticles = null;

	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );
	[SerializeField] Timer pullDuration = new Timer( 0.5f );
	[SerializeField] float range = 20.0f;

	bool canFire = true;

	GameObject c4Obj = null;
	[SerializeField] GameObject c4Prefab = null;
	[SerializeField] float throwStartDist = 1.0f;
	[SerializeField] float throwForce = 0.5f;
	[SerializeField] float explodeRange = 3.0f;
	[SerializeField] float explodeForce = 30.0f;
}
