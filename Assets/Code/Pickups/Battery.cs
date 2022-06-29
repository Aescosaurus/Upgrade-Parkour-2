using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery
	:
	MonoBehaviour
{
	void OnTriggerEnter( Collider coll )
	{
		if( !used && coll.gameObject.tag == "Player" )
		{
			coll.gameObject.GetComponent<PlayerMove2>().ReloadEquips();
			used = true;
			Destroy( gameObject );
		}
	}

	bool used = false;
}
