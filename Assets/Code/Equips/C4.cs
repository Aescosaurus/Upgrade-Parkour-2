using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4
	:
	ToolBase
{
	void Start()
	{
		controller = transform.Find( "C4Controller" ).gameObject;
		controllerOff = transform.Find( "C4ControllerOff" ).gameObject;

		refire.Update( refire.GetDuration() );
		ToggleIndicator( true );
	}

	void Update()
	{
		if( refire.Update( Time.deltaTime ) ) ToggleIndicator( true );
		if( SpiffyInput.CheckFree( inputKey ) )
		{
			if( c4Obj == null && refire.IsDone() )
			{
				refire.Reset();
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
				var playerForce = GetExplodeForce( playerMoveScr.gameObject );
				playerMoveScr.ApplyForceMove( playerForce * playerExplodeForce );

				var explodables = FindObjectsOfType<Explodable>();
				foreach( var ex in explodables )
				{
					if( ( ex.transform.position - c4Obj.transform.position ).sqrMagnitude < explodableHitRange * explodableHitRange )
					{
						ex.Explode( explodeForceMult );
					}
				}

				var interactives = GameObject.FindGameObjectsWithTag( "Interactive" );
				foreach( var interactive in interactives )
				{
					if( ( interactive.transform.position - c4Obj.transform.position ).sqrMagnitude < interactiveHitRange * interactiveHitRange )
					{
						var pushVec = interactive.transform.position - c4Obj.transform.position;
						interactive.GetComponent<Rigidbody>().AddForce( ( pushVec.normalized / pushVec.magnitude ) * interactiveForceMult +
							Vector3.up * interactiveUpForce,ForceMode.Impulse );
					}
				}

				Destroy( c4Obj );
				c4Obj = null;

				ToggleIndicator( false );
			}
		}
	}

	Vector3 GetExplodeForce( GameObject obj )
	{
		var dir = c4Obj.transform.position - obj.transform.position;
		float force = Mathf.Pow( explodeRange,2 ) - dir.sqrMagnitude;
		if( force > 0.0f ) return( ( -dir.normalized + Vector3.up * explodeUpBias ) * force * knockbackForce );
		
		return( Vector3.zero );
	}

	void ToggleIndicator( bool on )
	{
		controller.SetActive( on );
		controllerOff.SetActive( !on );
	}

	[SerializeField] float knockbackForce = 1.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );

	GameObject c4Obj = null;
	[SerializeField] GameObject c4Prefab = null;
	[SerializeField] float throwStartDist = 1.0f;
	[SerializeField] float throwForce = 0.5f;
	[SerializeField] float explodeRange = 3.0f;
	[SerializeField] float explodeUpBias = 0.8f;
	[SerializeField] float playerExplodeForce = 30.0f;

	[Header( "Explodable" )]
	[SerializeField] float explodeForceMult = 5.0f; // extra force applide on player when this explodes an explodable
	[SerializeField] float explodableHitRange = 10.0f;

	[Header( "Interactive" )]
	[SerializeField] float interactiveForceMult = 1.0f;
	[SerializeField] float interactiveHitRange = 10.0f;
	[SerializeField] float interactiveUpForce = 3.0f;

	GameObject controller;
	GameObject controllerOff;
}
