using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeWeaponBase
	:
	WeaponBase
{
	protected class RayReturnType
	{
		public Ray ray;
		public RaycastHit hit;
	}

	protected override void Start()
	{
		base.Start();

		bulletPrefab = Resources.Load<GameObject>( "Prefabs/Bullet" );
		bulletMask = ~LayerMask.GetMask( "EnemyBullet" );
	}

	protected override void Update()
	{
		base.Update();

		if( refire.IsDone() )
		{
			// animCtrl.SetBool( "aim",false );
		}
	}

	protected override void Fire()
	{
		animCtrl.SetBool( "aim",true );

		// check raycast and draw trail
		var rrt = CheckRayHit( shotDist );
		var trailLoc = rrt.hit.point;
		if( trailLoc == Vector3.zero ) trailLoc = rrt.ray.GetPoint( shotDist );
		SpawnTrail( trailLoc );
		// rrt.hit.transform?.GetComponent<EnemyBase>()?.Damage( 1.0f );
		rrt.hit.transform?.GetComponent<Damageable>()?.Damage( 1.0f );
	}

	// todo raycast from cam
	protected RayReturnType CheckRayHit( float dist )
	{
		Ray ray = new Ray( cam.transform.position,cam.transform.forward );
		RaycastHit hit;

		// SpawnTrail( ray.GetPoint( dist ) );

		Physics.Raycast( ray,out hit,dist,bulletMask );
		var rrt = new RayReturnType();
		rrt.ray = ray;
		rrt.hit = hit;
		return( rrt );
	}

	protected void SpawnTrail( Vector3 hitLoc )
	{
		var bullet = Instantiate( bulletPrefab );
		bullet.transform.position = hitLoc;
		var lr = bullet.GetComponent<LineRenderer>();
		lr.SetPosition( 0,transform.position );
		lr.SetPosition( 1,hitLoc );

		Destroy( bullet,1.0f );
	}

	// public override void ToggleAttacking( bool on )
	// {
	// 	base.ToggleAttacking( on );
	// 
	// 	animCtrl.SetBool( "aim",on );
	// }

	public override void CancelAttack()
	{
		base.CancelAttack();

		animCtrl.SetBool( "aim",false );
	}

	public override int GetPreferredHand()
	{
		return( 2 );
	}

	GameObject bulletPrefab;
	LayerMask bulletMask;

	[SerializeField] float shotDist = 10.0f;
}
