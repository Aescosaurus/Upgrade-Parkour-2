using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ThrowableWeapon
    :
    WeaponBase
{
	protected override void Start()
	{
		base.Start();

		// hurtArea = GetComponent<Collider>();
		// Assert.IsTrue( hurtArea.isTrigger );
		// hurtArea.enabled = false;
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
		// animCtrl.SetBool( "swing",true );
		// damagedEnemies.Clear();
		// attacking = true;

		var throwObj = hotbar.GetCurHeldPrefab();
		Assert.IsNotNull( throwObj );
		var toThrow = Instantiate( throwObj );
		toThrow.transform.position = hotbar.transform.position + cam.transform.forward + Vector3.up * 1.5f;
		toThrow.GetComponent<Rigidbody>().AddForce( cam.transform.forward * throwForce,ForceMode.Impulse );
		// print( toThrow );
		hotbar.ConsumeHeldItem();
	}

	public void ToggleHurtArea( bool on )
	{
		// hurtArea.enabled = on;
	}

	public override void ToggleAttacking( bool on )
	{
		base.ToggleAttacking( on );

		// hurtArea.enabled = on;
	}

	[SerializeField] float throwForce = 10.0f;
}
