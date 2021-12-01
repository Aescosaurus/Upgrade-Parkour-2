using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
	:
	MonoBehaviour
{
	enum State
	{
		Idle,
		Active
	}

	void Start()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
		body = GetComponent<Rigidbody>();
	}

	void Update()
	{
		switch( state )
		{
			case State.Idle:
				break;
			case State.Active:
				UpdateAI();
				break;
		}

		var diff = player.transform.position - transform.position;
		var diffXY = new Vector3( diff.x,0.0f,diff.z );
		if( diffXY.sqrMagnitude > activateDist * activateDist )
		{
			state = State.Idle;
		}
		if( diffXY.sqrMagnitude < activateDist * activateDist )
		{
			state = State.Active;
		}
	}

	protected virtual void InitAI() { }

	protected virtual void UpdateAI() { }

	protected GameObject player;
	protected Rigidbody body;

	[SerializeField] float activateDist = 70.0f;

	State state = State.Idle;
}
