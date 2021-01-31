using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ThrowableWeapon
    :
    WeaponBase
{
	// protected override void Update()
	// {
	// 	base.Update();
	// 
	// 	print( refire.GetPercent() );
	// }

	protected override void Fire()
	{
		// animCtrl.SetBool( "swing",true );
		// damagedEnemies.Clear();
		// attacking = true;

		var throwObj = hotbar.GetCurHeldPrefab();
		Assert.IsNotNull( throwObj );
		var toThrow = Instantiate( throwObj );
		toThrow.transform.position = hotbar.transform.position + cam.transform.forward + Vector3.up * 1.5f;
		toThrow.GetComponent<Rigidbody>().AddForce( cam.transform.forward * throwForce,ForceMode.Impulse );
		hotbar.ConsumeHeldItem();
	}

	public void ToggleHurtArea( bool on )
	{

	}

	[SerializeField] float throwForce = 10.0f;
}
