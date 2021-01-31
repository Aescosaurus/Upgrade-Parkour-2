using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		partHand = FindObjectOfType<ParticleHandler>();
		shirkTimer.Update( shirkTimer.GetDuration() );
		origScale = transform.localScale;
	}

	protected virtual void Update()
	{
		if( !shirkTimer.Update( Time.deltaTime ) )
		{
			var movePer = Mathf.Sin( shirkTimer.GetPercent() * Mathf.PI );
			var scale = transform.localScale;
			// scale.x = origScale.x + ( movePer * shirkScale.x );
			// scale.y = origScale.y + ( movePer * shirkScale.y );
			// scale.z = origScale.z + ( movePer * shirkScale.z );
			scale.x = Mathf.Lerp( origScale.x,origScale.x * shirkScale.x,movePer );
			scale.y = Mathf.Lerp( origScale.y,origScale.y * shirkScale.y,movePer );
			scale.z = Mathf.Lerp( origScale.z,origScale.z * shirkScale.z,movePer );
			transform.localScale = scale;
		}
		else transform.localScale = origScale;
	}

	public virtual void Damage( float amount )
	{
		amount -= def;
		shirkTimer.Reset();
		if( amount > 0.0f )
		{
			hp -= amount;

			if( hp <= 0.0f )
			{
				partHand.SpawnParticles( transform.position,( int )( ( amount + 0.5f ) * 15.0f ),oofFX );
				Destroy( gameObject );
			}
			else
			{
				partHand.SpawnParticles( transform.position,( int )( ( amount + 0.5f ) * 15.0f ),hitFX );
			}
		}
	}

	[SerializeField] protected float hp = 1.0f;
	[SerializeField] float def = 0.0f;
	[SerializeField] ParticleHandler.ParticleType hitFX = ParticleHandler.ParticleType.None;
	[SerializeField] ParticleHandler.ParticleType oofFX = ParticleHandler.ParticleType.None;
	[SerializeField] Vector3 shirkScale = new Vector3( 0.5f,1.5f,0.5f );

	protected ParticleHandler partHand;

	Timer shirkTimer = new Timer( 0.3f );
	Vector3 origScale;
}
