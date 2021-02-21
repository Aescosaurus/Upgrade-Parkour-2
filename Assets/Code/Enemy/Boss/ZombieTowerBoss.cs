using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTowerBoss
    :
    BossBase
{
	protected override void Start()
	{
		base.Start();

		boxColl = GetComponent<BoxCollider>();
	}

	protected override void Update()
	{
		base.Update();

		switch( phase )
		{
			case 0:
				{
					var diff = player.transform.position - transform.position;
					diff.y = 0.0f;
					Move( diff );
					if( diff.sqrMagnitude < Mathf.Pow( fireballDist,2 ) )
					{
						StopMoving();
						phase = 1;
						animCtrl.SetBool( "spin",true );
					}
				}
				break;
			case 1:
				if( fireballDuration.Update( Time.deltaTime ) )
				{
					phase = 2;
					animCtrl.SetBool( "spin",false );
					animCtrl.SetBool( "walk",true );
					fireballDuration.Reset();
				}
				else
				{
					if( fireballRefire.Update( Time.deltaTime ) )
					{
						fireballRefire.Reset();

						float randY = BoxPointSelector.GetRandPointWithinBox( boxColl ).y;
						var pos = transform.position;
						pos.y = randY;

						pos.x += Random.Range( -1.0f,1.0f );
						pos.z += Random.Range( -1.0f,1.0f );
						
						// float ang = Random.Range( 0.0f,360.0f );
						// var dir = new Vector3( Mathf.Cos( ang ),Mathf.Sin( ang ),0.0f ) * fireballSpeed;
						var dir = player.transform.position - pos;

						FireProjectile( fireballPrefab,pos,dir * fireballSpeed );
					}
				}
				break;
			case 2:

				break;
		}
	}

	int phase = 0;

	BoxCollider boxColl;

	[Header( "Fireball Phase" )]
	[SerializeField] float fireballDist = 10.0f;
	[SerializeField] Timer fireballRefire = new Timer( 0.2f );
	[SerializeField] Timer fireballDuration = new Timer( 3.0f );
	[SerializeField] GameObject fireballPrefab = null;
	[SerializeField] float fireballSpeed = 10.0f;
}
