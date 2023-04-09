using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBarArrow
	:
	MonoBehaviour
{
	void Start()
	{
		for( int i = 1; i < 5 + 1; ++i )
		{
			var bar = transform.parent.Find( "Bar" + i.ToString() );
			emptyBars[i - 1] = bar.Find( "BarEmpty" ).gameObject;
			fullBars[i - 1] = bar.Find( "BarFull" ).gameObject;
		}


	}

	const int barCount = 5;
	GameObject[] emptyBars = new GameObject[barCount];
	GameObject[] fullBars = new GameObject[barCount];
}