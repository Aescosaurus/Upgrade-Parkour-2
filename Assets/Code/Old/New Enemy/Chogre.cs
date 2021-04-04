using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chogre
    :
    NewEnemyBase
{
	protected override void Start()
	{
		base.Start();

		retarget.Update( retarget.GetDuration() );

		hitbox = transform.Find( "Body" ).GetComponentInChildren<BoxCollider>();
		hitbox.enabled = false;
	}

	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			if( !attacking )
			{
				if( retarget.Update( Time.deltaTime ) )
				{
					Transition( "walk",Random.Range( 0.0f,1.0f ) < 0.5f
						? "swing" : "strike" );

					attacking = true;
					hitbox.enabled = true;
				}
				else
				{
					animCtrl.SetBool( "walk",true );
				}

				Move( moveDir,true );
			}
		}
	}

	public override void StopAttacking()
	{
		base.StopAttacking();

		retarget.Reset();

		moveDir = ( player.transform.position - transform.position ).normalized;
		Transition( "swing","walk" );
		Transition( "strike","walk" );

		attacking = false;
		hitbox.enabled = false;
	}

	[SerializeField] Timer retarget = new Timer( 1.5f );

	Vector3 moveDir = Vector3.zero;

	bool attacking = false;

	BoxCollider hitbox;
}
