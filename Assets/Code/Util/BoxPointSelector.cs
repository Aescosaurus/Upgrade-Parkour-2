using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPointSelector
{
	public static Vector3 GetRandPointWithinBox( BoxCollider boxColl,float padding = 0.0f )
	{
		var point = Vector3.zero;
		var bounds = boxColl.bounds;

		point.x = Random.Range( bounds.min.x + padding,bounds.max.x - padding );
		point.y = Random.Range( bounds.min.y + padding,bounds.max.y - padding );
		point.z = Random.Range( bounds.min.z + padding,bounds.max.z - padding );
		// point = ( bounds.min + bounds.max ) / 2.0f;
		// Debug.Log( bounds.min + " - " + bounds.max );
		point = boxColl.transform.rotation * point;

		return( point + boxColl.transform.position );
	}
}
