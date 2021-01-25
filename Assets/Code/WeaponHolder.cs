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
			meleeWB = curWeapon.GetComponent<MeleeWeaponBase>();

			var handPref = curWB.GetPreferredHand();
			curWeapon.transform.parent = bh.GetHand( handPref ).transform;
			curWeapon.transform.localPosition = Vector3.zero;
			curWeapon.transform.Rotate( transform.right,90.0f * ( handPref == 1 ? 1 : -1 ) );
			curWB.LinkAnimator( GetComponent<Animator>() );
		}
	}

	void Update()
	{
		if( curWB.IsAttacking() )
		{
			SetRot();
		}
	}

	public void TryAttack( float aimDir )
	{
		if( curWB != null )
		{
			curWB.TryPerformAttack();
		}

		storedRot = aimDir;
		SetRot();
	}

	// int cuz anim events cant handle bool?
	public void TryToggleMeleeHurtArea( int on )
	{
		meleeWB?.ToggleHurtArea( on > 0 );
	}

	void SetRot()
	{
		var rot = transform.eulerAngles;
		// rot.y = Mathf.Atan2( xMove,yMove ) * Mathf.Rad2Deg;
		// rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
		rot.y = storedRot;
		transform.eulerAngles = rot;
	}

	public void SetTargetDir( float angle )
	{
		storedRot = angle;
	}

	[SerializeField] GameObject heldWeapon = null;

	GameObject curWeapon = null;
	WeaponBase curWB = null;
	MeleeWeaponBase meleeWB = null;

	float storedRot = 0.0f;
}
