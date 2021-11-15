using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4
	:
	ToolBase
{
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
				var playerForce = GetExplodeForce( playerMoveScr.gameObject );
				playerMoveScr.ApplyForceMove( playerForce );
				var explodables = FindObjectsOfType<Explodable>();
				foreach( var ex in explodables )
				{
					ex.GetComponent<Rigidbody>().AddForce( GetExplodeForce( ex.gameObject ) );
				}
				Destroy( c4Obj );
				c4Obj = null;
			}
		}
	}

	Vector3 GetExplodeForce( GameObject obj )
	{
		var dir = c4Obj.transform.position - obj.transform.position;
		float force = Mathf.Pow( explodeRange,2 ) - dir.sqrMagnitude;
		if( force > 0.0f ) return( -dir.normalized * force * explodeForce );
		
		return( Vector3.zero );
	}

	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );
	[SerializeField] float range = 20.0f;

	GameObject c4Obj = null;
	[SerializeField] GameObject c4Prefab = null;
	[SerializeField] float throwStartDist = 1.0f;
	[SerializeField] float throwForce = 0.5f;
	[SerializeField] float explodeRange = 3.0f;
	[SerializeField] float explodeForce = 30.0f;
}
