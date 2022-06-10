using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activateable
    :
    MonoBehaviour
{
    void Update()
	{
		if( startActive ) Activate();
	}

    public virtual void Activate()
	{

	}

	[SerializeField] bool startActive = true;
}
