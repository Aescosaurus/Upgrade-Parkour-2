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
			curWB = curWeapon.GetComponent<WeaponBase>();
			var handPref = curWB.GetPreferredHand();
			curWeapon.transform.parent = bh.GetHand( handPref ).transform;
			curWeapon.transform.localPosition = Vector3.zero;
			curWeapon.transform.Rotate( transform.right,90.0f * ( handPref == 1 ? 1 : -1 ) );
			curWB.LinkAnimator( GetComponent<Animator>() );
		}
	}

	public void TryAttack( float aimDir )
	{
		if( curWB.TryPerformAttack() )
		{
			var rot = transform.eulerAngles;
			// rot.y = Mathf.Atan2( xMove,yMove ) * Mathf.Rad2Deg;
			// rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
			rot.y = aimDir;
			transform.eulerAngles = rot;
		}
	}

	[SerializeField] GameObject heldWeapon = null;

	GameObject curWeapon = null;
	WeaponBase curWB;
}
