﻿using System.Collections;
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

		throwObj = hotbar.GetCurHeldPrefab();
		Assert.IsNotNull( throwObj );
		Instantiate( throwObj.transform.GetChild( 0 ),transform );
	}
	protected override void Update()
	{
		base.Update();

		if( refire.IsDone() )
		{
			animCtrl.SetBool( "throw",false );
		}
	}

	protected override void Fire()
	{
		animCtrl.SetBool( "throw",true );

		var toThrow = Instantiate( throwObj );
		toThrow.transform.position = hotbar.transform.position + cam.transform.forward + Vector3.up * 1.5f;
		toThrow.GetComponent<Rigidbody>().AddForce( cam.transform.forward * throwForce,ForceMode.Impulse );
		hotbar.ConsumeHeldItem();
	}

	public void ToggleHurtArea( bool on )
	{

	}

	[SerializeField] float throwForce = 10.0f;

	GameObject throwObj;
}
