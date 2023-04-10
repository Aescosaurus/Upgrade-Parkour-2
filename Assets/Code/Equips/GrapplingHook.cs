using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook
	:
	ToolBase
{
	void Start()
	{
		var player = transform.root.gameObject;
		charCtrl = player.GetComponent<CharacterController>();
		trailPrefab = ResLoader.Load( "Prefabs/Equips/GrapplingHookTrail" );
		hook = transform.Find( "GrapplingHook" ).gameObject;
		particlePrefab = ResLoader.Load( "Prefabs/Equips/GrappleParticles" );
		playerParticles = player.transform.Find( "Main Camera" ).Find( "GrappleParticles" ).gameObject;

		shotMask = ~LayerMask.GetMask( "Player","Pickup" );

		FireReset();
		Reload();

		if( !forceSetLevel ) UpdateLevel();
	}

	void Update()
	{
		refire[curLevel].Update( Time.deltaTime );
		if( SpiffyInput.CheckFree( inputKey ) )
		{
			if( canFire && refire[curLevel].IsDone() )
			{
				if( hitObj == null )
				{
					RaycastHit hit;
					if( Physics.Raycast( cam.transform.position,cam.transform.forward,out hit,range[curLevel],shotMask ) )
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

						hitObjIsExplodable = ( hitObj.GetComponent<Explodable>() != null );
						if( hitObj.tag == "Interactive" )
						{
							var knockbackDir = ( hitObj.position + hitOffset ) - transform.position;
							hitObj.GetComponent<Rigidbody>().AddForce( ( -knockbackDir.normalized * interactivePullForce[curLevel] +
								Vector3.up * interactiveUpForce[curLevel] ) * Time.deltaTime,ForceMode.Impulse );
						}
					}
				}
				else // hitobj not null -> grappling in progress
				{
					var hitPos = ( hitObj.position + hitOffset );
					var knockbackDir = hitPos - transform.position;
					playerMoveScr.ApplyForceMove( knockbackDir.normalized * pullForce[curLevel] * Time.deltaTime );

					curTrail.SetPosition( 0,transform.position );
					curTrail.SetPosition( 1,hitPos );

					if( hitObjIsExplodable && ( hitPos - transform.position ).sqrMagnitude < Mathf.Pow( explodableActivateDist,2 ) )
					{
						hitObj.GetComponent<Explodable>().Explode( explodeForceMult );
						FireReset();
					}

					if( pullDuration[curLevel].Update( Time.deltaTime ) )
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

			if( canFire && refire[curLevel].IsDone() ) hook.SetActive( true );
		}

		// if( charCtrl.isGrounded/* || true*/ )
		// {
		// 	if( !canFire )
		// 	{
		// 		// audSrc.PlayOneShot( reloadAud );
		// 		canFire = true;
		// 	}
		// }
	}

	void FireReset()
	{
		refire[curLevel].Reset();
		pullDuration[curLevel].Reset();

		// canFire = false;
		hitObj = null;
		hitOffset = Vector3.zero;

		playerMoveScr.ResetGrav();

		Destroy( curTrail?.gameObject );
		curTrail = null;
		Destroy( hookParticles );
		hookParticles = null;
		hook.SetActive( false );
		playerParticles.SetActive( false );
	}

	public override void Reload()
	{
		refire[curLevel].Update( refire[curLevel].GetDuration() );
		canFire = true;
		hook.SetActive( true );
	}

	public override void UpdateLevel()
	{
		curLevel = ToolManager.GetEquipLevel( PlayerMove2.Equip.GrapplingHook ) - 1;
		refire[curLevel].Update( refire[curLevel].GetDuration() );
	}

	LayerMask shotMask;
	CharacterController charCtrl;
	GameObject trailPrefab;
	GameObject particlePrefab;
	GameObject playerParticles;
	Transform hitObj = null;
	Vector3 hitOffset = Vector3.zero;
	LineRenderer curTrail = null;
	GameObject hook = null;
	GameObject hookParticles = null;

	[SerializeField] float[] pullForce = new float[ToolManager.levelCount];
	[SerializeField] Timer[] refire = new Timer[ToolManager.levelCount];
	[SerializeField] Timer[] pullDuration = new Timer[ToolManager.levelCount];
	[SerializeField] float[] range = new float[ToolManager.levelCount];

	[Header( "Explodable" )]
	[SerializeField] float explodableActivateDist = 5.0f;
	[SerializeField] float explodeForceMult = 5.0f;

	[Header( "Interactive" )]
	[SerializeField] float[] interactivePullForce = new float[ToolManager.levelCount];
	[SerializeField] float[] interactiveUpForce = new float[ToolManager.levelCount];

	bool canFire = true;
	bool hitObjIsExplodable = false;

	[SerializeField] int curLevel = 1;
	[SerializeField] bool forceSetLevel = false;
}
