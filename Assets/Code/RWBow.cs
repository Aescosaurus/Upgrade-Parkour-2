using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWBow
	:
	RangeWeaponBase
{
	protected override void Fire()
	{
		// check raycast and draw trail
		GameObject hitObj = CheckRayHit( shotDist );
		if( hitObj != null )
		{
			print( hitObj.name );
		}
	}

	[SerializeField] float shotDist = 10.0f;
}
