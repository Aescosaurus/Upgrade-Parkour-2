using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipHook
	:
	ToolBase
{
	void Start()
	{
		rayLoc = transform.Find( "RayLoc" );
		hook = transform.Find( "ZipHook" ).gameObject;
		hookOff = transform.Find( "ZipHookOff" ).gameObject;

		Reload();
	}
	
	void Update()
	{
		if( refire.Update( Time.deltaTime ) )
		{
			var wallRay = new Ray( rayLoc.position,rayLoc.forward );
			RaycastHit wallHit;
			if( Physics.Raycast( wallRay,out wallHit,range ) )
			{
				ToggleIndicator( true );

				if( SpiffyInput.CheckFree( inputKey ) )
				{
					refire.Reset();

					// jump
					var forward = cam.transform.forward;
					var up = cam.transform.up;
					var norm = wallHit.normal;

					playerMoveScr.ApplyForceMove( ( forward * jumpForwardBias +
						up * jumpUpBias + norm * jumpNormBias ) * jumpForce );
				}
			}
			else ToggleIndicator( false );
		}
		else ToggleIndicator( false );
	}
	
	void ToggleIndicator( bool on )
	{
		hook.SetActive( on );
		hookOff.SetActive( !on );
	}

	public override void Reload()
	{
		refire.Update( refire.GetDuration() );
		ToggleIndicator( true );
	}

	Transform rayLoc;
	GameObject hook;
	GameObject hookOff;

	[SerializeField] Timer refire = new Timer( 1.0f );
	[SerializeField] float range = 10.0f;

	[SerializeField] float jumpForce = 50.0f;
	[SerializeField] float jumpForwardBias = 0.5f;
	[SerializeField] float jumpUpBias = 0.7f;
	[SerializeField] float jumpNormBias = 0.2f;
}