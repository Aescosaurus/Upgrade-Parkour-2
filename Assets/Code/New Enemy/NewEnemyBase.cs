using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyBase
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		player = FindObjectOfType<PlayerBase>().gameObject;
		body = GetComponent<Rigidbody>();
		animCtrl = GetComponent<Animator>();
	}
	
	protected virtual void Update()
	{
		if( !activated && IsWithinRangeOf( player,activateRange ) )
		{
			Activate();
		}
	}

	// transition from state1 to state2
	protected void Transition( string state1,string state2 )
	{
		animCtrl.SetBool( state1,false );
		animCtrl.SetBool( state2,true );
	}

	protected virtual void Activate()
	{
		activated = true;
	}

	protected void Look( Vector3 dir )
	{
		dir.y = 0.0f;
		if( dir.sqrMagnitude > 0.0f )
		{
			transform.forward = dir;
		}
	}

	protected virtual void Move( Vector3 dir,bool slowRotate = false )
	{
		dir.y = 0.0f;

		if( slowRotate )
		{
			var rot = transform.eulerAngles;
			rot.y = Mathf.Atan2( dir.x,dir.z ) * Mathf.Rad2Deg;
			rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
			transform.eulerAngles = rot;
		}

		body.AddForce( dir.normalized * accel );
		if( body.velocity.sqrMagnitude > maxSpeed * maxSpeed )
		{
			body.velocity = body.velocity.normalized * maxSpeed;
		}
	}

	protected GameObject FireProjectile( GameObject prefab,Vector3 pos,Vector3 aim )
	{
		var proj = Instantiate( prefab );

		var projScr = proj.GetComponent<Projectile>();

		proj.GetComponent<Collider>().isTrigger = true;
		proj.transform.position = pos;
		proj.transform.forward = aim;
		proj.GetComponent<Rigidbody>().AddForce( aim.normalized * projScr.GetShotSpd(),ForceMode.Impulse );
		proj.layer = LayerMask.NameToLayer( "EnemyBullet" );

		proj.GetComponent<Projectile>().SetTeam( 2 );

		Destroy( proj.GetComponent<LoadableItem>() );
		Destroy( proj.GetComponent<ItemPickup>() );

		return ( proj );
	}

	protected bool IsWithinRangeOf( GameObject target,float range )
	{
		var dist = target.transform.position - transform.position;
		return ( dist.sqrMagnitude < Mathf.Pow( range,2 ) );
	}

	void OnTriggerEnter( Collider coll )
	{
		if( !appliedDamage )
		{
			var damageable = coll.GetComponent<DamageablePlayer>();
			if( damageable != null )
			{
				damageable.Damage( transform.forward,triggerDamage );
				appliedDamage = true;
			}
		}
	}

	void OnTriggerExit( Collider coll )
	{
		appliedDamage = false;
	}

	protected GameObject player;
	protected Rigidbody body;
	protected Animator animCtrl;

	[SerializeField] float activateRange = 15.0f;
	[SerializeField] float accel = 100.0f;
	[SerializeField] float maxSpeed = 5.0f;
	[SerializeField] float rotSpeed = 2.4f;
	[SerializeField] float triggerDamage = 1.0f;
	
	bool appliedDamage = false;

	protected bool activated = false;
}
