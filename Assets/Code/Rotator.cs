using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator
	:
	Activateable
{
	public override void Activate()
	{
		transform.Rotate( rotDir,rotSpeed * Time.deltaTime );
	}

	[SerializeField] Vector3 rotDir = Vector3.zero;
	[SerializeField] float rotSpeed = 10.0f;
}
