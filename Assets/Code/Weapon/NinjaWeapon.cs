using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaWeapon
    :
    MeleeWeapon
{
	protected override void Update()
	{
		base.Update();

		if( refire.IsDone() )
		{
			// hurtArea.enabled = false;
			animCtrl.SetBool( "strike",false );
			attacking = false;
		}
	}

	protected override void Fire()
	{
		// animCtrl.SetBool( "swing",true );
		// damagedEnemies.Clear();
		// attacking = true;
		base.Fire();
		animCtrl.SetBool( "swing",false );
		animCtrl.SetBool( "strike",false );
	}
}
