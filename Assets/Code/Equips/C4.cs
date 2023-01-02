using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4
	:
	ExplosiveToolBase
{
	void Start()
	{
		controller = transform.Find( "C4Controller" ).gameObject;
		controllerOff = transform.Find( "C4ControllerOff" ).gameObject;

		Reload();
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

				CauseExplosion( c4Obj.transform.position );

				Destroy( c4Obj );
				c4Obj = null;

				ToggleIndicator( false );
			}
		}
	}

	void ToggleIndicator( bool on )
	{
		controller.SetActive( on );
		controllerOff.SetActive( !on );
	}

	public override void Reload()
	{
		refire.Update( refire.GetDuration() );
		ToggleIndicator( true );
	}

	Vector3 GetExplodeForce( GameObject obj )
	{
		var dir = c4Obj.transform.position - obj.transform.position;
		float force = Mathf.Pow( explodeRange,2 ) - dir.sqrMagnitude;
		if( force > 0.0f ) return ( ( -dir.normalized + Vector3.up * explodeUpBias ) * force * knockbackForce );

		return ( Vector3.zero );
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

	GameObject controller;
	GameObject controllerOff;
}
