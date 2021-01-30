using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPointSelector
{
	public static Vector3 GetRandPointWithinBox( BoxCollider boxColl )
	{
		var point = Vector3.zero;
		var bounds = boxColl.bounds;

		point.x = Random.Range( bounds.min.x,bounds.max.x );
		point.y = Random.Range( bounds.min.y,bounds.max.y );
		point.z = Random.Range( bounds.min.z,bounds.max.z );
		// point = ( bounds.min + bounds.max ) / 2.0f;

		return( point + boxColl.transform.position );
	}
}
