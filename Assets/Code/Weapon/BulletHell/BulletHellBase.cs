using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BulletHellBase
	:
	WeaponBase
{
	protected override void Start()
	{
		base.Start();

		shotSpot = transform.Find( "ShotSpot" );
		Assert.IsNotNull( shotSpot );
	}

	protected override void Fire()
	{
		var proj = Instantiate( projectile );

		var projScr = proj.GetComponent<Projectile>();
		projScr.SetTeam( team );

		proj.GetComponent<Collider>().isTrigger = true;
		// proj.transform.position = animCtrl.transform.position + Vector3.up * 1.2f + animCtrl.transform.forward;
		proj.transform.position = shotSpot.position;
		proj.transform.forward = cam.transform.forward/* + Vector3.up * upAimBias*/;
		proj.GetComponent<Rigidbody>().AddForce( proj.transform.forward * projScr.GetShotSpd(),ForceMode.Impulse );
		proj.layer = LayerMask.NameToLayer( team == 1 ? "Default" : "EnemyBullet" );

		Destroy( proj.GetComponent<LoadableItem>() );
		Destroy( proj.GetComponent<ItemPickup>() );
	}

	[SerializeField] GameObject projectile = null;

	Transform shotSpot;
}
