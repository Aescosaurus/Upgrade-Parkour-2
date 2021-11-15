using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBase
	:
	MonoBehaviour
{
	public void SetInputKey( string key )
	{
		inputKey = key;
	}

	protected string inputKey = "Fire1";
}
