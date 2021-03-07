using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalInteract
	:
	InteractiveBase
{
	protected override void Start()
	{
		base.Start();

		partHand = FindObjectOfType<ParticleHandler>();

		if( Random.Range( 0.0f,1.0f ) < decoChance ) Destroy( this );
		else
		{
			Destroy( GetComponent<Damageable>() );
			CavernGenerator.CollectDeco( -1 );
		}
	}

	protected override void Interact()
	{
		CavernGenerator.CollectDeco( 1 );
		partHand.SpawnParticles( transform.position,35,ParticleHandler.ParticleType.Spark );
		Destroy( gameObject );
	}

	[SerializeField] float decoChance = 0.5f;

	ParticleHandler partHand;
}
