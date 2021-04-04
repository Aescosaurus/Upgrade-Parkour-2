using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBase
	:
	NewEnemyBase
{
	protected override void Start()
	{
		base.Start();

		hitbox = transform.Find( "Body" ).Find( "Hitbox" ).GetComponent<BoxCollider>();
		hitbox.enabled = false;
	}

	protected override void Update()
	{
		base.Update();
	}

	public virtual void HopStart()
	{
		hitbox.enabled = true;
	}

	public virtual void HopEnd()
	{
		hitbox.enabled = false;
	}

	protected virtual void Hop( Vector3 dir )
	{
		dir.y = 0.0f;
		transform.forward = dir;

		body.AddForce( dir.normalized * hopForce + Vector3.up * hopHeight,ForceMode.Impulse );
	}

	void OnTriggerEnter( Collider coll )
	{
		coll.GetComponent<DamageablePlayer>()?.Damage( transform.forward,1.0f );
	}

	[SerializeField] protected float hopForce = 10.0f;
	[SerializeField] protected float hopHeight = 0.3f;

	BoxCollider hitbox;
}
