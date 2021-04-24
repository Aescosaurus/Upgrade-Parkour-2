using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamRotator
	:
	MonoBehaviour
{
	void FixedUpdate()
	{
		var rot = transform.eulerAngles;
		rot.y = Mathf.Lerp( start,end,( Mathf.Cos( Time.time * speed ) + 1.0f ) / 2.0f );
		transform.eulerAngles = rot;
	}

	[SerializeField] float start = 0.0f;
	[SerializeField] float end = 80.0f;
	[SerializeField] float speed = 0.001f;
}
