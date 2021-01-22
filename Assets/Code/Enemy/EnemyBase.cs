using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase
	:
	MonoBehaviour
{
	public void Damage( float amount )
	{
		hp -= amount;

		if( hp < 0.0f ) Destroy( gameObject );
	}

	[SerializeField] float hp = 10.0f;
}
