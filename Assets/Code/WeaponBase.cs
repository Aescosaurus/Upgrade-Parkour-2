using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		animCtrl = FindObjectOfType<PlayerWalk>().GetComponent<Animator>();
	}

	void Update()
	{
		if( refire.Update( Time.deltaTime ) && Input.GetAxis( "Fire1" ) > 0.0f )
		{
			animCtrl.SetFloat( "shot_spd",1.0f / refire.GetDuration() );
			refire.Reset();
			Fire();
		}
	}

	protected abstract void Fire();

	Animator animCtrl;

	[SerializeField] Timer refire = new Timer( 0.5f );
}
