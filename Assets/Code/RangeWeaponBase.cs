using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeWeaponBase
	:
	WeaponBase
{
	void Start()
	{
		cam = Camera.main;
		bulletPrefab = Resources.Load<GameObject>( "Prefabs/Bullet" );
	}

	// todo raycast from cam
	protected GameObject CheckRayHit( float dist )
	{
		Ray ray = new Ray( cam.transform.position,cam.transform.forward );
		RaycastHit hit;

		SpawnTrail( ray.GetPoint( dist ) );

		if( Physics.Raycast( ray,out hit,dist ) )
		{
			return( hit.transform.gameObject );
		}
		else
		{
			return( null );
		}
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
}
