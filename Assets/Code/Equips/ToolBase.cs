using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolBase
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

	public abstract void Reload();

	protected Camera cam;
	protected PlayerMove2 playerMoveScr;

	protected string inputKey = "Fire1";
}
