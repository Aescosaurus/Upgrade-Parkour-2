﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Damageable
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		partHand = FindObjectOfType<ParticleHandler>();
		shirkTimer.Update( shirkTimer.GetDuration() );
		origScale = transform.localScale;

		hp = maxHP;

		audSrc = gameObject.AddComponent<AudioSource>();
		body = GetComponent<Rigidbody>();
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

	public virtual void Damage( Vector3 forceDir,float amount )
	{
		amount -= def;
		shirkTimer.Reset();
		if( amount > 0.0f )
		{
			// audSrc.PlayOneShot( ouchSound );
			hp -= amount;

			body.AddForce( forceDir * knockbackForce + Vector3.up * knockbackForceUp,ForceMode.Impulse );

			var curFX = hitFX;
			if( hp <= 0.0f )
			{
				curFX = oofFX;
				Oof();
			}
			partHand.SpawnParticles( transform.position,( int )( ( amount + 0.5f ) * 15.0f ),curFX );
		}
	}

	public void Damage( float amount )
	{
		Assert.IsTrue( false );
	}

	protected virtual void Oof()
	{
		if( !oofed )
		{
			oofed = true;
			// if( oofSound != null )
			// {
			// 	// audSrc.PlayOneShot( oofSound );
			// 	var audLeftover = Instantiate( ResLoader.Load( "Prefabs/AudioLeftover" ) );
			// 	audLeftover.AddComponent<AudioSource>().PlayOneShot( oofSound );
			// 	Destroy( audLeftover,oofSound.length );
			// }
			// else print( "Oof sound is null on " + gameObject.name );

			var enemies = FindObjectsOfType<NewEnemyBase>();
			if( enemies.Length <= 1 )
			{
				XPUI.Win();
			}

			Destroy( gameObject );
		}
	}

	public void Heal( float amount )
	{
		hp += amount;
		if( hp > maxHP ) hp = maxHP;

		partHand.SpawnParticles( transform.position,( int )( ( amount + 0.5f ) * 15.0f ),hitFX );
	}

	public void AddMaxHP( float amount )
	{
		maxHP += amount;
		hp += amount;
	}

	public int GetTeam()
	{
		return( team );
	}

	public float GetHPPercent()
	{
		return( hp / maxHP );
	}

	[SerializeField] int team = 2;
	[SerializeField] protected float maxHP = 1.0f;
	protected float hp;
	[SerializeField] float def = 0.0f;
	[SerializeField] ParticleHandler.ParticleType hitFX = ParticleHandler.ParticleType.None;
	[SerializeField] ParticleHandler.ParticleType oofFX = ParticleHandler.ParticleType.None;
	[SerializeField] Vector3 shirkScale = new Vector3( 0.8f,1.2f,0.8f );

	protected ParticleHandler partHand;

	Timer shirkTimer = new Timer( 0.3f );
	Vector3 origScale;
	protected bool oofed = false;

	protected AudioSource audSrc;
	Rigidbody body;
	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] float knockbackForceUp = 3.0f;
	// [SerializeField] AudioClip ouchSound = null;
	// [SerializeField] AudioClip oofSound = null;
}
