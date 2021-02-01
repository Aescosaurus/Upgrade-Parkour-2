using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShard
	:
	LoadableItem
{
	public void RandomToss()
	{
		var body = GetComponent<Rigidbody>();

		var randMove = Vector3.up;
		randMove.x += Random.Range( -1.0f,1.0f ) * flyDev;
		randMove.y += Random.Range( -1.0f,1.0f ) * flyDev;
		randMove.z += Random.Range( -1.0f,1.0f ) * flyDev;

		body.AddForceAtPosition( randMove * jumpForce,transform.position + Vector3.down );
	}

	[SerializeField] float jumpForce = 10.0f;
	[SerializeField] float flyDev = 0.2f;
}
