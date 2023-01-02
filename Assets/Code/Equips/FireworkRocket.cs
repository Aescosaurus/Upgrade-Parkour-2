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
	}

	void Update()
	{
		if( flying )
		{
			playerMoveScr.ApplyForceMove( ( cam.transform.forward + Vector3.up * flyUpBias ) * flySpd * Time.deltaTime );

			if( flyDur.Update( Time.deltaTime ) )
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
			refire.Update( Time.deltaTime );
			if( refire.IsDone() )
			{
				ToggleIndicator( true );

				if( SpiffyInput.CheckFree( inputKey ) )
				{
					refire.Reset();
					flyDur.Reset();
					flying = true;
					flyingParts.SetActive( true );
					ToggleIndicator( false );
				}
			}
		}
	}

	public override void Reload()
	{
		if( flying ) flyDur.Reset(); // extend flying duration if already flying
		else
		{
			refire.Update( refire.GetDuration() );
			ToggleIndicator( true );
		}

		// todo: update indicators
	}

	void ToggleIndicator( bool on )
	{
		fuse.SetActive( on );
	}

	GameObject fuse;
	GameObject flyingParts;

	bool flying = false;

	[SerializeField] Timer refire = new Timer( 5.0f );
	[SerializeField] Timer flyDur = new Timer( 1.0f );
	[SerializeField] float flySpd = 10.0f;
	[SerializeField] float flyUpBias = 3.0f;

	[SerializeField] int explodePartCount = 20;
	[SerializeField] float explodeSpawnDist = 3.0f;
}
