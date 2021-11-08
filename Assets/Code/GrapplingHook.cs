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
		playerMoveScr = player.GetComponent<PlayerMove2>();
		charCtrl = player.GetComponent<CharacterController>();
		trailPrefab = ResLoader.Load( "Prefabs/GrapplingHookTrail" );
		hook = transform.Find( "GrapplingHook" ).gameObject;
		particlePrefab = ResLoader.Load( "Prefabs/GrappleParticles" );
		playerParticles = player.transform.Find( "Main Camera" ).Find( "GrappleParticles" ).gameObject;

		shotMask = LayerMask.GetMask( "Default" );

		// bulletPrefab = ResLoader.Load( "Prefabs/ShotgunTrail" );
		// shotLoc = transform.Find( "ShotLoc" );
		// 
		// audSrc = GetComponent<AudioSource>();
		// 
		// shootAud = Resources.Load<AudioClip>( "Audio/ShotgunShoot" );
		// reloadAud = Resources.Load<AudioClip>( "Audio/ShotgunReload" );

		FireReset();
	}

	void Update()
	{
		refire.Update( Time.deltaTime );
		if( SpiffyInput.CheckFree( inputKey ) )
		{
			if( canFire && refire.IsDone() )
			{
				if( hitObj == null )
				{
					RaycastHit hit;
					if( Physics.Raycast( cam.transform.position,cam.transform.forward,out hit,range,shotMask ) )
					{
						hitObj = hit.transform;
						hitOffset = hit.point - hitObj.position;

						Destroy( curTrail?.gameObject );
						curTrail = Instantiate( trailPrefab ).GetComponent<LineRenderer>();

						Destroy( hookParticles );
						hookParticles = Instantiate( particlePrefab );
						hookParticles.transform.position = hit.point;
						hookParticles.transform.up = ( hit.point - transform.position );

						playerParticles.SetActive( true );

						hook.SetActive( false );

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
					var hitPos = ( hitObj.position + hitOffset );
					var knockbackDir = hitPos - transform.position;
					playerMoveScr.ApplyForceMove( knockbackDir.normalized * knockbackForce );

					curTrail.SetPosition( 0,transform.position );
					curTrail.SetPosition( 1,hitPos );

					if( pullDuration.Update( Time.deltaTime ) )
					{
						FireReset();
					}
				}
			}
		}
		else
		{
			if( hitObj != null )
			{
				FireReset();
			}
		}

		if( charCtrl.isGrounded || true )
		{
			if( !canFire )
			{
				// audSrc.PlayOneShot( reloadAud );
				canFire = true;
				hook.SetActive( true );
			}
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

		playerMoveScr.ResetGrav();

		Destroy( curTrail?.gameObject );
		curTrail = null;
		Destroy( hookParticles );
		hookParticles = null;
		playerParticles.SetActive( false );
	}

	public void SetInputKey( string key )
	{
		inputKey = key;
	}

	Camera cam;
	PlayerMove2 playerMoveScr;
	LayerMask shotMask;
	CharacterController charCtrl;
	GameObject trailPrefab;
	GameObject particlePrefab;
	GameObject playerParticles;
	// GameObject bulletPrefab;
	// Transform shotLoc;
	// AudioSource audSrc;
	// AudioClip shootAud;
	// AudioClip reloadAud;
	Transform hitObj = null;
	Vector3 hitOffset = Vector3.zero;
	LineRenderer curTrail = null;
	GameObject hook = null;
	GameObject hookParticles = null;

	[SerializeField] float knockbackForce = 10.0f;
	[SerializeField] Timer refire = new Timer( 0.1f );
	[SerializeField] Timer pullDuration = new Timer( 0.5f );
	[SerializeField] float range = 20.0f;
	// [SerializeField] float bulletDespawn = 0.3f;
	// [SerializeField] RangeI pelletCount = new RangeI( 3,5 );
	// [SerializeField] float pelletSpread = 0.7f;
	// [SerializeField] float minSpread = 0.5f;

	bool canFire = true;

	string inputKey = "Fire1";
}
