using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook
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

		// bulletPrefab = ResLoader.Load( "Prefabs/ShotgunTrail" );
		// shotLoc = transform.Find( "ShotLoc" );
		// 
		// audSrc = GetComponent<AudioSource>();
		// 
		// shootAud = Resources.Load<AudioClip>( "Audio/ShotgunShoot" );
		// reloadAud = Resources.Load<AudioClip>( "Audio/ShotgunReload" );
	}

	void Update()
	{
		refire.Update( Time.deltaTime );
		if( SpiffyInput.CheckFree( "Fire2" ) )
		{
			if( canFire && refire.IsDone() )
			{
				if( hitObj == null )
				{
					RaycastHit hit;
					if( Physics.Raycast( cam.transform.position,cam.transform.forward,out hit,range,shotMask ) )
					{
						hitObj = hit.transform;
						hitOffset = hitObj.position - hit.point + Vector3.up * upBias;

						// canFire = false;
						// refire.Reset();

						// var curPellets = pelletCount.Rand();
						// for( int i = 0; i < curPellets; ++i )
						// {
						// 	SpawnTrail( hit.point + new Vector3(
						// 		Random.Range( 0.0f,1.0f ),
						// 		Random.Range( 0.0f,1.0f ),
						// 		Random.Range( 0.0f,1.0f ) ) *
						// 		Mathf.Max( minSpread,pelletSpread * Mathf.Sqrt( hit.distance ) ) );
						// }
						// 
						// audSrc.PlayOneShot( shootAud );
					}
				}
				else
				{
					var knockbackDir = ( hitObj.position + hitOffset ) - transform.position;
					playerMoveScr.ApplyForceMove( knockbackDir.normalized * knockbackForce );

					if( pullDuration.Update( Time.deltaTime ) )
					{
						FireReset();
					}
				}
			}
		}
		else
		{
			// FireReset();
			hitObj = null;
		}

		if( charCtrl.isGrounded )
		{
			// if( !canFire ) audSrc.PlayOneShot( reloadAud );
			canFire = true;
		}
	}

	// void SpawnTrail( Vector3 hitLoc )
	// {
	// 	var bullet = Instantiate( bulletPrefab );
	// 	bullet.transform.position = hitLoc;
	// 	var lr = bullet.GetComponent<LineRenderer>();
	// 	lr.SetPosition( 0,shotLoc.position );
	// 	lr.SetPosition( 1,hitLoc );
	// 
	// 	Destroy( bullet,bulletDespawn );
	// }

	void FireReset()
	{
		refire.Reset();
		pullDuration.Reset();
		canFire = false;
		hitObj = null;
		hitOffset = Vector3.zero;
	}

	Camera cam;
	PlayerMove playerMoveScr;
	LayerMask shotMask;
	CharacterController charCtrl;
	// GameObject bulletPrefab;
	// Transform shotLoc;
	// AudioSource audSrc;
	// AudioClip shootAud;
	// AudioClip reloadAud;
	Transform hitObj = null;
	Vector3 hitOffset = Vector3.zero;

	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );
	[SerializeField] Timer pullDuration = new Timer( 0.5f );
	[SerializeField] float range = 20.0f;
	[SerializeField] float upBias = 1.0f;
	// [SerializeField] float bulletDespawn = 0.3f;
	// [SerializeField] RangeI pelletCount = new RangeI( 3,5 );
	// [SerializeField] float pelletSpread = 0.7f;
	// [SerializeField] float minSpread = 0.5f;

	bool canFire = true;
}
