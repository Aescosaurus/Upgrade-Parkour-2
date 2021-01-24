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
			curWeapon = Instantiate( heldWeapon );
			var wb = curWeapon.GetComponent<WeaponBase>();
			var handPref = wb.GetPreferredHand();
			curWeapon.transform.parent = bh.GetHand( handPref ).transform;
			curWeapon.transform.localPosition = Vector3.zero;
			curWeapon.transform.Rotate( transform.right,90.0f * ( handPref == 1 ? 1 : -1 ) );
			wb.LinkAnimator( GetComponent<Animator>() );
		}
	}

	[SerializeField] GameObject heldWeapon = null;

	GameObject curWeapon = null;
}
