using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon
	:
	WeaponBase
{
	protected override void Fire()
	{
		animCtrl.SetBool( "aim",CanFire() );
	}

	public override void CancelAttack()
	{
		base.CancelAttack();

		animCtrl.SetBool( "aim",false );
	}

	public override int GetPreferredHand()
	{
		return( 2 );
	}

	public override void LinkAnimator( Animator animCtrl )
	{
		base.LinkAnimator( animCtrl );

		invHand = animCtrl.GetComponent<PlayerInventory>()?.GetInv();
	}

	public override void ToggleAttacking( bool on )
	{
		base.ToggleAttacking( on );
		
		if( on ) FireProjectile( projectilePrefab,shotSpeed,damage,0.2f );
	}

	bool CanFire()
	{
		return( invHand == null || invHand.TryConsumeItem( ammoItem,ammoUsage ) );
	}

	InventoryHandler invHand;

	[SerializeField] GameObject projectilePrefab = null;
	[SerializeField] LoadableItem ammoItem = null;
	[SerializeField] int ammoUsage = 1;
	[SerializeField] float shotSpeed = 10.0f;
	[SerializeField] float damage = 1.0f;
}
