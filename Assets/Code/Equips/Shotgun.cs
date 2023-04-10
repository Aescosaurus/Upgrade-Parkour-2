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

		indicator = transform.Find( "ShotgunIndicator" ).gameObject;
		indicatorOff = transform.Find( "ShotgunIndicatorOff" ).gameObject;
		Reload();

		if( !forceSetLevel ) UpdateLevel();
	}

	void Update()
	{
		refire[curLevel].Update( Time.deltaTime );
		if( SpiffyInput.CheckAxis( inputKey ) && canFire && refire[curLevel].IsDone() )
		{
			var knockbackDir = -cam.transform.forward;
			RaycastHit hit;
			if( Physics.Raycast( cam.transform.position,cam.transform.forward,out hit,999.0f,shotMask ) )
			{
				// var knockForce = knockbackDir.normalized * Mathf.Min( knockbackForce * ( 2.0f / hit.distance ),maxForce );
				// var knockForce = knockbackDir.normalized / hit.distance;
				var nerfAmount = Mathf.Max( hit.distance - distNerfStart[curLevel],1.0f );
				// var nerfAmount = hit.distance;
				// if( nerfAmount < distNerfStart * distNerfStart ) nerfAmount = 1.0f;
				// else nerfAmount = hit.distance - distNerfStart;

				var knockForce = knockbackDir.normalized / nerfAmount;

				hit.transform.GetComponent<Explodable>()?.Explode();
				if( hit.transform.tag == "Interactive" )
				{
					hit.transform.GetComponent<Rigidbody>().AddForce( -knockForce * interactiveKnockback[curLevel],ForceMode.Impulse );
				}
				playerMoveScr.ApplyForceMove( knockForce * knockbackForce[curLevel] );

				canFire = false;
				refire[curLevel].Reset();

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

			ToggleIndicator( false );
		}

		if( charCtrl.isGrounded )
		{
			if( !canFire ) audSrc.PlayOneShot( reloadAud );
			canFire = true;
			if( refire[curLevel].IsDone() ) ToggleIndicator( true );
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

	void ToggleIndicator( bool on )
	{
		indicator.SetActive( on );
		indicatorOff.SetActive( !on );
	}

	public override void Reload()
	{
		refire[curLevel].Update( refire[curLevel].GetDuration() );
		canFire = true;
		ToggleIndicator( true );
	}

	public override void UpdateLevel()
	{
		curLevel = ToolManager.GetEquipLevel( PlayerMove2.Equip.Shotgun ) - 1;
		refire[curLevel].Update( refire[curLevel].GetDuration() );
	}

	LayerMask shotMask;
	CharacterController charCtrl;
	GameObject bulletPrefab;
	Transform shotLoc;
	AudioSource audSrc;
	AudioClip shootAud;
	AudioClip reloadAud;
	GameObject indicator;
	GameObject indicatorOff;

	[SerializeField] float[] knockbackForce = new float[ToolManager.levelCount];
	[SerializeField] Timer[] refire = new Timer[ToolManager.levelCount];

	[SerializeField] float bulletDespawn = 0.3f;
	[SerializeField] RangeI pelletCount = new RangeI( 3,5 );
	[SerializeField] float pelletSpread = 0.7f;
	[SerializeField] float minSpread = 0.5f;
	// distance at which falloff starts applying
	[SerializeField] float[] distNerfStart = new float[ToolManager.levelCount];

	[SerializeField] float[] interactiveKnockback = new float[ToolManager.levelCount];

	bool canFire = true;

	[SerializeField] int curLevel = 1;
	[SerializeField] bool forceSetLevel = false;
}
