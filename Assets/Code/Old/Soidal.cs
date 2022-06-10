using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soidal
	:
	Activateable
{
	void Start()
	{
		startLoc = transform.position;
	}

	void Update()
	{
		transform.position = startLoc + transform.TransformDirection( moveAxis ) *
			Mathf.Sin( Time.time * moveSpeed ) * moveDist;
	}

	[SerializeField] Vector3 moveAxis = Vector3.zero;
	[SerializeField] float moveDist = 5.0f;
	[SerializeField] float moveSpeed = 1.0f;

	Vector3 startLoc;
}
