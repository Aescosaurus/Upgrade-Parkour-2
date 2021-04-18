using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun
	:
	MonoBehaviour
{
	void Start()
	{
		cam = Camera.main;

		var player = transform.root.gameObject;
		playerMoveScr = player.GetComponent<PlayerMove>();
		charCtrl = player.GetComponent<CharacterController>();

		shotMask = LayerMask.GetMask( "Default" );

		bulletPrefab = ResLoader.Load( "Prefabs/ShotgunTrail" );
		shotLoc = transform.Find( "ShotLoc" );
	}

	void Update()
	{
		refire.Update( Time.deltaTime );
		if( SpiffyInput.CheckAxis( "Fire1" ) && canFire && refire.IsDone() )
		{
			var knockbackDir = -cam.transform.forward;
			RaycastHit hit;
			if( Physics.Raycast( cam.transform.position,cam.transform.forward,out hit,999.0f,shotMask ) )
			{
				playerMoveScr.ApplyForceMove( knockbackDir.normalized * Mathf.Min(
					knockbackForce * ( 1.0f / hit.distance ),maxForce ) );

				canFire = false;
				refire.Reset();

				var curPellets = pelletCount.Rand();
				for( int i = 0; i < curPellets; ++i )
				{
					SpawnTrail( hit.point + new Vector3(
						Random.Range( 0.0f,1.0f ),
						Random.Range( 0.0f,1.0f ),
						Random.Range( 0.0f,1.0f ) ) *
						Mathf.Max( minSpread,pelletSpread * Mathf.Sqrt( hit.distance ) ) );
				}
			}
		}

		if( charCtrl.isGrounded )
		{
			canFire = true;
		}
	}

	void SpawnTrail( Vector3 hitLoc )
	{
		var bullet = Instantiate( bulletPrefab );
		bullet.transform.position = hitLoc;
		var lr = bullet.GetComponent<LineRenderer>();
		lr.SetPosition( 0,shotLoc.position );
		lr.SetPosition( 1,hitLoc );

		Destroy( bullet,bulletDespawn );
	}

	Camera cam;
	PlayerMove playerMoveScr;
	LayerMask shotMask;
	CharacterController charCtrl;
	GameObject bulletPrefab;
	Transform shotLoc;

	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] float maxForce = 500.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );
	[SerializeField] float bulletDespawn = 0.3f;
	[SerializeField] RangeI pelletCount = new RangeI( 3,5 );
	[SerializeField] float pelletSpread = 0.7f;
	[SerializeField] float minSpread = 0.5f;

	bool canFire = true;
}
