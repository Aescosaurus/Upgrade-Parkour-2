using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase
	:
	MonoBehaviour
{
	void Start()
	{
		Destroy( gameObject,lifetime );
	}

	[SerializeField] float lifetime = 5.0f;
}
