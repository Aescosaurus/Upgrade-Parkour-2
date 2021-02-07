using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs
	:
	Portal
{
	protected override void Interact()
	{
		// todo check coins first

		int curFloor = PlayerPrefs.GetInt( "dungeonfloor",0 );
		++curFloor;
		PlayerPrefs.SetInt( "dungeonfloor",curFloor );

		base.Interact();
	}
}
