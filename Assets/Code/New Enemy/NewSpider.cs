using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpider
	:
	NewEnemyBase
{
	protected override void Start()
	{
		base.Start();

		hitbox = transform.Find( "Hitbox" ).GetComponent<BoxCollider>();
		hitbox.gameObject.SetActive( false );
	}

	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			var dir = player.transform.position - transform.position;
			dir.y = 0.0f;
			if( !lunging )
			{
				if( dir.sqrMagnitude > strafeDist * strafeDist )
				{
					Move( dir,true );
				}
				else if( dir.sqrMagnitude < fleeDist * fleeDist )
				{
					Move( -dir,true );
				}
				else
				{
					if( strafeDir == 0 ) strafeDir = Random.Range( -1,2 );

					Move( CalcMoveDir( dir ) );
					Look( dir );

					if( lungeTimer.Update( Time.deltaTime ) )
					{
						lunging = true;
						lungeTimer.Reset();
						Transition( "walk","lunging" );
						appliedDamage = false;
					}
				}
			}
			else
			{
				Look( dir );
			}
		}
	}

	protected override void Move( Vector3 dir,bool slowRotate = false )
	{
		base.Move( dir,slowRotate );

		animCtrl.SetBool( "walk",true );
	}

	Vector3 CalcMoveDir( Vector3 orig )
	{
		if( strafeDir > 0 ) return ( new Vector3( orig.z,0.0f,-orig.x ) );
		else if( strafeDir < 0 ) return ( new Vector3( -orig.z,0.0f,orig.x ) );
		else return ( orig );
	}

	// Call when wind up is done and ready to apply force.
	public void BeginLunge()
	{
		transform.position += Vector3.up * 0.2f;
		body.AddForce( ( ( player.transform.position - transform.position ).normalized + Vector3.up * lungeUpBias ) * lungePower,
			ForceMode.Impulse );
		hitbox.gameObject.SetActive( true );

		// audSrc.PlayOneShot( jumpSound );
	}

	void OnCollisionEnter( Collision coll )
	{
		if( animCtrl != null )
		{
			if( lunging )
			{
				strafeDir = Random.Range( -1,2 );
				hitbox.gameObject.SetActive( false );
			}

			lunging = false;
			// animCtrl.SetBool( "lunging",false );
			Transition( "lunging","walk" );
		}
	}

	void OnTriggerEnter( Collider coll )
	{
		if( !appliedDamage )
		{
			var damageable = coll.GetComponent<DamageablePlayer>();
			if( damageable != null )
			{
				damageable.Damage( transform.forward,lungeDamage );
				appliedDamage = true;
			}
		}
	}

	[SerializeField] float strafeDist = 5.0f;
	[SerializeField] float fleeDist = 3.0f;
	int strafeDir = 0;
	[SerializeField] Timer lungeTimer = new Timer( 2.0f );
	bool lunging = false;
	[SerializeField] float lungePower = 5.0f;
	[SerializeField] float lungeUpBias = 0.2f;
	BoxCollider hitbox;
	[SerializeField] float lungeDamage = 2.0f;
	bool appliedDamage = false;

	// [SerializeField] AudioClip jumpSound = null;
}
