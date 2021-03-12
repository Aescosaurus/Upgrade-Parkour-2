using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSword
	:
	WeaponBase
{
	protected override void Start()
	{
		base.Start();

		// projectilePrefab = ResLoader.Load( "Prefabs/Weapon/Projectile/FlameHurt" );
	}

	protected override void Fire()
	{
		animCtrl.SetBool( "swing",true );
	}

	public override void CancelAttack()
	{
		base.CancelAttack();

		animCtrl.SetBool( "swing",false );
	}

	public override void ToggleAttacking( bool on )
	{
		base.ToggleAttacking( on );

		if( on ) FireProjectile( projectilePrefab,shotSpeed,damage );
	}

	[SerializeField] GameObject projectilePrefab;
	[SerializeField] float shotSpeed = 30.0f;
	[SerializeField] float damage = 1.0f;
}
