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
		cam = Camera.main;
		bulletPrefab = Resources.Load<GameObject>( "Prefabs/Bullet" );
		bulletMask = ~LayerMask.GetMask( "EnemyBullet" );
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

	Camera cam;
	GameObject bulletPrefab;
	LayerMask bulletMask;
}
