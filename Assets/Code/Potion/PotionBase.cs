using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotionBase
	:
	WeaponBase
{
	protected override void Fire()
	{
		animCtrl.SetBool( "drink",true );
	}

	public override void ToggleAttacking( bool on )
	{
		base.ToggleAttacking( on );

		if( on )
		{
			Drink();
			hotbar.ConsumeHeldItem();
		}
	}

	protected abstract void Drink();
}
