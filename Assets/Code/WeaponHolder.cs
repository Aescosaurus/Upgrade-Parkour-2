using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder
	:
	MonoBehaviour
{
	void Awake()
	{
		bh = GetComponent<BipedHandler>();
		animCtrl = GetComponent<Animator>();
		if( heldWeapon != null && bh != null )
		{
			InitNewWeapon( heldWeapon );
		}

		hotbar = FindObjectOfType<HotbarHandler>();
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
		storedRot = aimDir;
		SetRot();

		if( curWB != null )
		{
			curWB.TryPerformAttack();
		}
	}

	public void CancelAttack()
	{
		curWB.CancelAttack();
	}

	// int cuz anim events cant handle bool?
	// public void TryToggleMeleeHurtArea( int on )
	// {
	// 	meleeWB?.ToggleHurtArea( on > 0 );
	// }

	public void ToggleAttacking( int on )
	{
		curWB.ToggleAttacking( on == 1 );
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

	public void ReplaceWeapon( GameObject replacement )
	{
		curWB.CancelAttack();
		Destroy( curWB.gameObject );
		InitNewWeapon( replacement );
		CancelAnims();
	}

	void InitNewWeapon( GameObject prefab )
	{
		var curWeapon = Instantiate( prefab );
		curWB = curWeapon.GetComponent<WeaponBase>();
		curWB.SetHotbar( hotbar );
		// meleeWB = curWeapon.GetComponent<MeleeWeaponBase>();

		var interactive = curWeapon.GetComponent<InteractiveBase>();
		if( interactive != null ) interactive.enabled = false;

		var handPref = curWB.GetPreferredHand();
		// curWeapon.transform.parent = bh.GetHand( handPref ).transform;
		curWeapon.transform.SetParent( bh.GetHand( handPref ).transform,false );
		curWeapon.transform.Rotate( Vector3.right,90.0f * ( handPref == 1 ? 1 : -1 ) );
		curWB.LinkAnimator( animCtrl );
	}

	void CancelAnims()
	{
		animCtrl.SetBool( "aim",false );
		animCtrl.SetBool( "swing",false );
		animCtrl.SetBool( "throw",false );
		animCtrl.SetBool( "drink",false );
	}

	public void StopAttacking()
	{
		CancelAnims();
		curWB.CancelAttack();
	}

	public GameObject GetWeapon()
	{
		return( heldWeapon );
	}

	public bool Attacking()
	{
		return( curWB.IsAttacking() );
	}

	[SerializeField] GameObject heldWeapon = null;

	// GameObject curWeapon = null;
	WeaponBase curWB = null;
	// MeleeWeaponBase meleeWB = null;
	BipedHandler bh;
	Animator animCtrl;

	float storedRot = 0.0f;

	HotbarHandler hotbar;
}
