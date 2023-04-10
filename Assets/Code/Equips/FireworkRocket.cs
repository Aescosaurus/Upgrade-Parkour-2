using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkRocket
	:
	ExplosiveToolBase
{
	void Start()
	{
		fuse = transform.Find( "FireworkRocketFuse" ).gameObject;
		
		flyingParts = transform.Find( "FlyingParticles" ).gameObject;
		flyingParts.SetActive( false );

		Reload();

		if( !forceSetLevel ) UpdateLevel();
	}

	void Update()
	{
		if( flying )
		{
			playerMoveScr.ApplyForceMove( ( cam.transform.forward + Vector3.up * flyUpBias ) * flySpd[curLevel] * Time.deltaTime );

			if( flyDur[curLevel].Update( Time.deltaTime ) )
			{
				flying = false;
				flyingParts.SetActive( false );
				CauseExplosion( transform.position );
				PartHand.SpawnParts( transform.position + cam.transform.forward * explodeSpawnDist,
					explodePartCount,PartHand.PartType.FireworkRocket );
			}
		}
		else
		{
			refire[curLevel].Update( Time.deltaTime );
			if( refire[curLevel].IsDone() )
			{
				ToggleIndicator( true );

				if( SpiffyInput.CheckFree( inputKey ) )
				{
					refire[curLevel].Reset();
					flyDur[curLevel].Reset();
					flying = true;
					flyingParts.SetActive( true );
					ToggleIndicator( false );
				}
			}
		}
	}

	public override void Reload()
	{
		if( flying ) flyDur[curLevel].Reset(); // extend flying duration if already flying
		else
		{
			refire[curLevel].Update( refire[curLevel].GetDuration() );
			ToggleIndicator( true );
		}

		// todo: update indicators
	}

	void ToggleIndicator( bool on )
	{
		fuse.SetActive( on );
	}

	public override void UpdateLevel()
	{
		curLevel = ToolManager.GetEquipLevel( PlayerMove2.Equip.FireworkRocket ) - 1;
		refire[curLevel].Update( refire[curLevel].GetDuration() );
	}

	GameObject fuse;
	GameObject flyingParts;

	bool flying = false;

	[SerializeField] Timer[] refire = new Timer[ToolManager.levelCount];
	[SerializeField] Timer[] flyDur = new Timer[ToolManager.levelCount];
	[SerializeField] float[] flySpd = new float[ToolManager.levelCount];
	[SerializeField] float flyUpBias = 3.0f;

	[SerializeField] int explodePartCount = 20;
	[SerializeField] float explodeSpawnDist = 3.0f;

	[SerializeField] int curLevel = 1;
	[SerializeField] bool forceSetLevel = false;
}
