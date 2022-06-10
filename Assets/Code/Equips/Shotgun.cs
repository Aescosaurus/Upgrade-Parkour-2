using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun
	:
	ToolBase
{
	void Start()
	{
		var player = transform.root.gameObject;
		charCtrl = player.GetComponent<CharacterController>();

		shotMask = ~LayerMask.GetMask( "Player" );

		bulletPrefab = ResLoader.Load( "Prefabs/Equips/ShotgunTrail" );
		shotLoc = transform.Find( "ShotLoc" );

		audSrc = GetComponent<AudioSource>();

		shootAud = Resources.Load<AudioClip>( "Audio/ShotgunShoot" );
		reloadAud = Resources.Load<AudioClip>( "Audio/ShotgunReload" );
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
				var knockForce = knockbackDir.normalized * Mathf.Min( knockbackForce * ( 2.0f / hit.distance ),maxForce );
				// if( hit.transform.gameObject.GetComponent<Explodable>() == null )
				{
					playerMoveScr.ApplyForceMove( knockForce );
				}
				// else
				// {
				// 	hit.transform.GetComponent<Rigidbody>().AddForce( -knockForce * hitForceMult,ForceMode.Impulse );
				// }

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

				audSrc.PlayOneShot( shootAud );
			}
		}

		if( charCtrl.isGrounded )
		{
			if( !canFire ) audSrc.PlayOneShot( reloadAud );
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

	LayerMask shotMask;
	CharacterController charCtrl;
	GameObject bulletPrefab;
	Transform shotLoc;
	AudioSource audSrc;
	AudioClip shootAud;
	AudioClip reloadAud;

	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] float maxForce = 500.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );
	[SerializeField] float bulletDespawn = 0.3f;
	[SerializeField] RangeI pelletCount = new RangeI( 3,5 );
	[SerializeField] float pelletSpread = 0.7f;
	[SerializeField] float minSpread = 0.5f;

	bool canFire = true;
}
