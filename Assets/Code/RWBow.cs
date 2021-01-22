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
		var rrt = CheckRayHit( shotDist );
		var trailLoc = rrt.hit.point;
		if( trailLoc == Vector3.zero ) trailLoc = rrt.ray.GetPoint( shotDist );
		SpawnTrail( trailLoc );
		rrt.hit.transform?.GetComponent<EnemyBase>()?.Damage( 1.0f );
	}

	[SerializeField] float shotDist = 10.0f;
}
