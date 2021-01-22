using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
	:
	MonoBehaviour
{
	void Update()
	{
		if( refire.Update( Time.deltaTime ) && Input.GetAxis( "Fire1" ) > 0.0f )
		{
			refire.Reset();
			Fire();
		}
	}

	protected abstract void Fire();

	[SerializeField] Timer refire = new Timer( 0.5f );
}
