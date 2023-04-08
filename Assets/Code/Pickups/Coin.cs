using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin
	:
	MonoBehaviour
{
	void OnTriggerEnter( Collider coll )
	{
		if( coll.tag == "Player" && !collected )
		{
			collected = true;

			StatsPanel.CollectCoin( value );

			Destroy( gameObject );
		}
	}

	bool collected = false;

	[SerializeField] int value = 1;
}