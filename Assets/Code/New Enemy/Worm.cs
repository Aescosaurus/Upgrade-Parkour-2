using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm
    :
    NewEnemyBase
{
	protected override void Start()
	{
		base.Start();

		model = transform.Find( "Body" ).gameObject;
		// particles = transform.Find( "Particles" ).gameObject;
		hitbox = GetComponent<BoxCollider>();
		hitbox.isTrigger = true;
		partHand = FindObjectOfType<ParticleHandler>();

		// particles.SetActive( false );

		retargetTimer.Update( retargetTimer.GetDuration() );
	}

	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			if( burrowed )
			{
				if( retargetTimer.Update( Time.deltaTime ) )
				{
					retargetTimer.Reset();
					targetPos = player.transform.position;
				}

				if( chompTimer.Update( Time.deltaTime ) )
				{
					chompTimer.Reset();

					Transition( "burrow","surface" );
					// particles.SetActive( false );
					model.SetActive( true );
					hitbox.enabled = true;
				}
				else
				{
					var diff = targetPos - transform.position;
					Move( diff );
				}
			}
		}
	}

	public void Burrow()
	{
		burrowed = true;
		// particles.SetActive( true );
		model.SetActive( false );

		hitbox.enabled = false;
		partHand.SpawnParticles( transform.position,20,ParticleHandler.ParticleType.Worm );
	}

	public void Surface()
	{
		Transition( "surface","burrow" );
		burrowed = false;
	}

	protected override void Activate()
	{
		base.Activate();

		animCtrl.SetBool( "burrow",true );
	}

	[SerializeField] Timer chompTimer = new Timer( 7.0f );
	[SerializeField] Timer retargetTimer = new Timer( 2.0f );

	GameObject model;
	// GameObject particles;
	BoxCollider hitbox;
	ParticleHandler partHand;

	bool burrowed = false;
	Vector3 targetPos = Vector3.zero;
}
