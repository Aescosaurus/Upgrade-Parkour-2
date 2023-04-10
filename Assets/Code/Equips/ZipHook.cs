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

		if( !forceSetLevel ) UpdateLevel();
	}
	
	void Update()
	{
		if( refire[curLevel].Update( Time.deltaTime ) )
		{
			var wallRay = new Ray( rayLoc.position,rayLoc.forward );
			RaycastHit wallHit;
			if( Physics.Raycast( wallRay,out wallHit,range[curLevel] ) )
			{
				ToggleIndicator( true );

				if( SpiffyInput.CheckFree( inputKey ) )
				{
					refire[curLevel].Reset();

					// jump
					var forward = cam.transform.forward;
					var up = cam.transform.up;
					var norm = wallHit.normal;

					playerMoveScr.ApplyForceMove( ( forward * jumpForwardBias +
						up * jumpUpBias + norm * jumpNormBias ) * jumpForce[curLevel] );
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
		refire[curLevel].Update( refire[curLevel].GetDuration() );
		ToggleIndicator( true );
	}

	public override void UpdateLevel()
	{
		curLevel = ToolManager.GetEquipLevel( PlayerMove2.Equip.ZipHook ) - 1;
		refire[curLevel].Update( refire[curLevel].GetDuration() );
	}

	Transform rayLoc;
	GameObject hook;
	GameObject hookOff;

	[SerializeField] Timer[] refire = new Timer[ToolManager.levelCount];
	[SerializeField] float[] range = new float[ToolManager.levelCount];

	[SerializeField] float[] jumpForce = new float[ToolManager.levelCount];
	[SerializeField] float jumpForwardBias = 0.5f;
	[SerializeField] float jumpUpBias = 0.7f;
	[SerializeField] float jumpNormBias = 0.2f;

	[SerializeField] int curLevel = 1;
	[SerializeField] bool forceSetLevel = false;
}