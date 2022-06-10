using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBase
	:
	MonoBehaviour
{
	void Awake()
	{
		cam = Camera.main;

		var player = transform.root.gameObject;
		playerMoveScr = player.GetComponent<PlayerMove2>();
	}

	public void SetInputKey( string key )
	{
		inputKey = key;
	}

	protected Camera cam;
	protected PlayerMove2 playerMoveScr;

	protected string inputKey = "Fire1";
}
