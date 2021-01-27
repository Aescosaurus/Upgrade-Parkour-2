﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MeleeWeaponBase
	:
	WeaponBase
{
	protected override void Start()
	{
		base.Start();

		hurtArea = GetComponent<Collider>();
		Assert.IsTrue( hurtArea.isTrigger );
		hurtArea.enabled = false;
	}

	protected override void Update()
	{
		base.Update();

		if( refire.IsDone() )
		{
			// hurtArea.enabled = false;
			animCtrl.SetBool( "swing",false );
			attacking = false;
		}
	}

	protected override void Fire()
	{
		// StopCoroutine( HandleAttack( 0.0f ) );
		// StartCoroutine( HandleAttack( refire.GetDuration() ) );

		animCtrl.SetBool( "swing",true );
		// hurtArea.enabled = true;
		damagedEnemies.Clear();
		attacking = true;
	}

	// IEnumerator HandleAttack( float s )
	// {
	// 	animCtrl.SetBool( "swing",true );
	// 	hurtArea.enabled = true;
	// 	yield return( new WaitForSeconds( s ) );
	// 	hurtArea.enabled = false;
	// 	animCtrl.SetBool( "swing",false );
	// }

	void OnTriggerEnter( Collider coll )
	{
		var enemyScr = coll.GetComponent<Damageable>();
		var playerScr = coll.GetComponent<PlayerWalk>();
		if( team == 1 && enemyScr != null &&
			!damagedEnemies.Contains( enemyScr ) )
		{
			enemyScr.Damage( damage );
			damagedEnemies.Add( enemyScr );
		}
		else if( playerScr != null )
		{
			// damage player
		}
	}

	public void ToggleHurtArea( bool on )
	{
		hurtArea.enabled = on;
		// attacking = on;
	}

	public override void ToggleAttacking( bool on )
	{
		base.ToggleAttacking( on );

		hurtArea.enabled = on;
	}

	Collider hurtArea;

	[SerializeField] float damage = 1.0f;

	List<Damageable> damagedEnemies = new List<Damageable>();
}
