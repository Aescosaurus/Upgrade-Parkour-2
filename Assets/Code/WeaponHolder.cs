using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder
	:
	MonoBehaviour
{
	void Start()
	{
		var bh = GetComponent<BipedHandler>();
		if( heldWeapon != null && bh != null )
		{
			curWeapon = Instantiate( heldWeapon,bh.GetHand( 1 ).transform );
			curWeapon.transform.Rotate( transform.right,90.0f );
		}
	}

	[SerializeField] GameObject heldWeapon = null;

	GameObject curWeapon = null;
}
