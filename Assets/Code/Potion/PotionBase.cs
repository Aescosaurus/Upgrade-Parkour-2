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
		GetComponent<BoxCollider>().enabled = false;
	}

	public override void ToggleAttacking( bool on )
	{
		base.ToggleAttacking( on );

		if( on )
		{
			Drink();
			// potion drinking sfx
			hotbar.ConsumeHeldItem();
		}
	}

	protected abstract void Drink();
}
