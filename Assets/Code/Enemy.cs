using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
	:
	MonoBehaviour
{
	enum State
	{
		Idle,
		Chase,
		Scatter
	}

	void Start()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
		body = GetComponent<Rigidbody>();

		RandomizeScatterMove();
	}

	void Update()
	{
		var diff = player.transform.position - transform.position;
		var diffXY = new Vector3( diff.x,0.0f,diff.z );

		switch( state )
		{
			case State.Idle:
				break;
			case State.Chase:
				if( body.velocity.sqrMagnitude < maxSpd * maxSpd ||
					Vector3.Dot( diffXY,body.velocity ) < 0.0f ) // vel facing away from diff
				{
					body.AddForce( diffXY.normalized * accel * Time.deltaTime );
				}
				break;
			case State.Scatter:
				if( body.velocity.sqrMagnitude < maxSpd * maxSpd )
				{
					body.AddForce( scatterMove * accel * Time.deltaTime );
				}

				if( scatterPeriod.Update( Time.deltaTime ) )
				{
					RandomizeScatterMove();
					scatterPeriod.Reset();
				}
				break;
		}

		bool aboveScatterHeight = Mathf.Abs( diff.y ) > scatterHeight;
		if( diffXY.sqrMagnitude > activateDist * activateDist )
		{
			state = State.Idle;
		}
		if( diffXY.sqrMagnitude < activateDist * activateDist &&
			( !aboveScatterHeight || diffXY.sqrMagnitude > chaseDist * chaseDist ) )
		{
			state = State.Chase;
		}
		if( diffXY.sqrMagnitude < scatterDist * scatterDist && aboveScatterHeight )
		{
			state = State.Scatter;
		}
	}

	void RandomizeScatterMove()
	{
		scatterMove.Set(
			Random.Range( -1.0f,1.0f ),
			0.0f,
			Random.Range( -1.0f,1.0f ) );
		scatterMove.Normalize();
	}

	GameObject player;
	Rigidbody body;

	[SerializeField] float activateDist = 50.0f;
	[SerializeField] float chaseDist = 50.0f;
	[SerializeField] float accel = 10.0f;
	[SerializeField] float maxSpd = 10.0f;
	[SerializeField] float scatterDist = 10.0f;
	[SerializeField] float scatterHeight = 5.0f;
	[SerializeField] Timer scatterPeriod = new Timer( 2.0f );

	State state = State.Idle;
	Vector3 scatterMove = Vector3.zero;
}
