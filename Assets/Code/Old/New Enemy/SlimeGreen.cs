using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGreen
    :
    SlimeBase
{
	protected override void Start()
	{
		base.Start();

		StartCoroutine( JumpRest( Random.Range( 0.0f,restDuration ) ) );
	}

	protected override void Update()
	{
		base.Update();

		if( activated )
		{
			var diff = player.transform.position - transform.position;
			Look( diff );
		}
	}

	public override void HopStart()
	{
		base.HopStart();

		Vector3 hopDir;

		if( activated )
		{
			hopDir = player.transform.position - transform.position;
			Look( hopDir );
		}
		else
		{
			hopDir = new Vector3(
				Random.Range( -1.0f,1.0f ),
				0.0f,
				Random.Range( -1.0f,1.0f ) );
		}

		Hop( hopDir );
	}

	public override void HopEnd()
	{
		base.HopEnd();

		Transition( "hop","idle" );
		StartCoroutine( JumpRest( restDuration ) );
	}

	// protected override void Activate()
	// {
	// 	Transition( "idle","hop" );
	// }

	IEnumerator JumpRest( float t )
	{
		yield return( new WaitForSeconds( t ) );

		Transition( "idle","hop" );
	}

	[SerializeField] float restDuration = 1.0f;
}
