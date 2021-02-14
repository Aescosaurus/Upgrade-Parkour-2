using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon
	:
	WeaponBase
{
	protected override void Fire()
	{
		animCtrl.SetBool( "aim",true );

		var proj = Instantiate( projectilePrefab );
		proj.transform.position = animCtrl.transform.position + Vector3.up * 1.3f + animCtrl.transform.forward;
		proj.transform.forward = animCtrl.transform.forward + Vector3.up * 0.2f;
		proj.GetComponent<Rigidbody>().AddForce( animCtrl.transform.forward * shotSpeed,ForceMode.Impulse );
		var projScr = proj.GetComponent<Projectile>();
		projScr.SetDamage( damage );
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

	[SerializeField] GameObject projectilePrefab = null;
	[SerializeField] float shotSpeed = 10.0f;
	[SerializeField] float damage = 1.0f;
}
