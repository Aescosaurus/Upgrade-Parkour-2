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

		if( !forceSetLevel ) UpdateLevel();
	}

	void Update()
	{
		if( refire[curLevel].Update( Time.deltaTime ) ) ToggleIndicator( true );
		if( SpiffyInput.CheckFree( inputKey ) )
		{
			if( c4Obj == null && refire[curLevel].IsDone() )
			{
				refire[curLevel].Reset();
				c4Obj = Instantiate( c4Prefab/*,playerMoveScr.transform*/ );
				c4Obj.transform.position = cam.transform.position + cam.transform.forward * throwStartDist;
				var rb = c4Obj.GetComponent<Rigidbody>();
				rb.velocity = playerMoveScr.GetVel();
				rb.AddForce( cam.transform.forward * throwForce[curLevel],ForceMode.Impulse );
			}
		}
		else
		{
			if( c4Obj != null )
			{
				// explode
				var playerForce = GetExplodeForce( playerMoveScr.gameObject );
				playerMoveScr.ApplyForceMove( playerForce * playerExplodeForce[curLevel] );

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
		refire[curLevel].Update( refire[curLevel].GetDuration() );
		ToggleIndicator( true );
	}

	Vector3 GetExplodeForce( GameObject obj )
	{
		var dir = c4Obj.transform.position - obj.transform.position;
		float force = Mathf.Pow( explodeRange[curLevel],2 ) - dir.sqrMagnitude;
		if( force > 0.0f ) return ( ( -dir.normalized + Vector3.up * explodeUpBias[curLevel] ) * force * knockbackForce[curLevel] );

		return ( Vector3.zero );
	}

	public override void UpdateLevel()
	{
		curLevel = ToolManager.GetEquipLevel( PlayerMove2.Equip.C4 ) - 1;
		refire[curLevel].Update( refire[curLevel].GetDuration() );
	}

	[SerializeField] float[] knockbackForce = new float[ToolManager.levelCount];
	[SerializeField] Timer[] refire = new Timer[ToolManager.levelCount];

	GameObject c4Obj = null;
	[SerializeField] GameObject c4Prefab = null;
	[SerializeField] float throwStartDist = 1.0f;
	[SerializeField] float[] throwForce = new float[ToolManager.levelCount];
	[SerializeField] float[] explodeRange = new float[ToolManager.levelCount];
	[SerializeField] float[] explodeUpBias = new float[ToolManager.levelCount];
	[SerializeField] float[] playerExplodeForce = new float[ToolManager.levelCount];

	GameObject controller;
	GameObject controllerOff;

	[SerializeField] int curLevel = 1;
	[SerializeField] bool forceSetLevel = false;
}
