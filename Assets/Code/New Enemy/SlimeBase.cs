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
	}

	protected override void Update()
	{
		base.Update();
	}

	public virtual void HopStart()
	{

	}

	public virtual void HopEnd()
	{

	}

	protected virtual void Hop( Vector3 dir )
	{
		dir.y = 0.0f;
		transform.forward = dir;

		body.AddForce( dir.normalized * hopForce + Vector3.up * hopHeight,ForceMode.Impulse );
	}

	[SerializeField] protected float hopForce = 10.0f;
	[SerializeField] protected float hopHeight = 0.3f;
}
