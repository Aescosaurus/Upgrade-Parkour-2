using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
	:
	MonoBehaviour
{
	void OnTriggerEnter( Collider coll )
	{
		var damageTarget = coll.GetComponent<Damageable>();
		var targetTeam = -1;
		if( damageTarget != null )
		{
			targetTeam = damageTarget.GetTeam();
			if( targetTeam != team ) damageTarget.Damage( damage );
		}

		if( targetTeam != team ) Destroy( gameObject );
	}

	public void SetDamage( float amount )
	{
		damage = amount;
	}

	public void SetTeam( int team )
	{
		this.team = team;
	}

	float damage = 0.0f;
	int team = -1;
}
