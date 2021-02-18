using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
	:
	LoadableItem
{
	protected virtual void Start()
	{
		cam = Camera.main;

		// animCtrl = FindObjectOfType<PlayerWalk>().GetComponent<Animator>();
		team = animCtrl?.GetComponent<EnemyBase>() != null ? 2 : 1;

		refire.Update( refire.GetDuration() );
	}

	protected virtual void Update()
	{
		// todo enemy attack ai
		// if( refire.Update( Time.deltaTime ) &&
		// 	( team == 2 || Input.GetAxis( "Fire1" ) > 0.0f ) )
		// {
		// 	Fire();
		// 	animCtrl.SetFloat( "shot_spd",1.0f / refire.GetDuration() );
		// 	refire.Reset();
		// }
		refire.Update( Time.deltaTime );
	}

	protected abstract void Fire();

	public virtual void LinkAnimator( Animator animCtrl )
	{
		this.animCtrl = animCtrl;
	}

	public bool TryPerformAttack()
	{
		bool done = refire.IsDone();

		if( done )
		{
			animCtrl.SetFloat( "shot_spd",1.0f / refire.GetDuration() );
			Fire();
			refire.Reset();
		}

		return( done );
	}

	public virtual void ToggleAttacking( bool on )
	{
		attacking = on;
	}

	public virtual void CancelAttack()
	{
		attacking = false;
	}

	public void SetHotbar( HotbarHandler hotbar )
	{
		this.hotbar = hotbar;
	}

	protected void FireProjectile( GameObject projectile,float shotSpeed,float damage,float upAimBias = 0.1f )
	{
		var proj = Instantiate( projectile );
		proj.GetComponent<Collider>().isTrigger = true;
		proj.transform.position = animCtrl.transform.position + Vector3.up * 1.2f + animCtrl.transform.forward;
		proj.transform.forward = cam.transform.forward + Vector3.up * upAimBias;
		proj.GetComponent<Rigidbody>().AddForce( proj.transform.forward * shotSpeed,ForceMode.Impulse );

		var projScr = proj.GetComponent<Projectile>();
		projScr.SetDamage( damage );
		projScr.SetTeam( team );

		Destroy( proj.GetComponent<LoadableItem>() );
		Destroy( proj.GetComponent<ItemPickup>() );
	}

	public virtual int GetPreferredHand()
	{
		return( 1 );
	}

	public bool IsAttacking()
	{
		return( attacking );
	}

	protected Animator animCtrl;

	[SerializeField] protected Timer refire = new Timer( 0.5f );

	protected int team = -1; // 1 = player 2 = enemy

	protected bool attacking = false;

	protected HotbarHandler hotbar = null;
	protected Camera cam;
}
