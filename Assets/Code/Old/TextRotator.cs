using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotator
	:
	MonoBehaviour
{
	void Start()
	{
		cam = Camera.main;
		player = GameObject.Find( "Player" );
		mesh = GetComponent<MeshRenderer>();
	}

	void Update()
	{
		var dist = player.transform.position - transform.position;
		bool draw = dist.sqrMagnitude < drawDist * drawDist;
		mesh.enabled = draw;
		if( draw )
		{
			// var rot = transform.eulerAngles;
			// var diff = cam.transform.position - transform.position;
			// rot.y = Mathf.Atan2( diff.x,diff.z ) * Mathf.Rad2Deg + 180.0f;
			// transform.eulerAngles = rot;
			transform.forward = cam.transform.forward;
		}
	}

	Camera cam;
	GameObject player;
	[SerializeField] float drawDist = 10.0f;
	MeshRenderer mesh;
}
