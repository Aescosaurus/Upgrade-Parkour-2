using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon
	:
	WeaponBase
{
	protected override void Fire()
	{
		if( CanFire() )
		{
			animCtrl.SetBool( "aim",true );

			// var proj = Instantiate( projectilePrefab );
			// proj.GetComponent<Collider>().isTrigger = true;
			// proj.transform.position = animCtrl.transform.position + Vector3.up * 1.3f + animCtrl.transform.forward;
			// proj.transform.forward = animCtrl.transform.forward + Vector3.up * 0.2f;
			// proj.GetComponent<Rigidbody>().AddForce( animCtrl.transform.forward * shotSpeed,ForceMode.Impulse );
			// 
			// var projScr = proj.GetComponent<Projectile>();
			// projScr.SetDamage( damage );
			// 
			// Destroy( proj.GetComponent<LoadableItem>() );
			// Destroy( proj.GetComponent<ItemPickup>() );
			FireProjectile( projectilePrefab,shotSpeed,damage,0.2f );
		}
		else
		{
			animCtrl.SetBool( "aim",false );
		}
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
