using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BipedHandler
	:
	MonoBehaviour
{
	// 1 = right 2 = left
	public GameObject GetHand( int side )
	{
		Assert.IsTrue( side == 1 || side == 2 );
		return( transform.Find( "Body" ).Find( "Arm" + side.ToString() ).GetChild( 0 ).gameObject );
	}
}
