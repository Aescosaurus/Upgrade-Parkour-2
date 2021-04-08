using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator
	:
	MonoBehaviour
{
	void OnTriggerStay( Collider coll )
	{
		target.GetComponent<Activateable>()?.Activate();
	}

	[SerializeField] GameObject target = null;
}
