using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyBase
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		player = FindObjectOfType<PlayerBase>().gameObject;
		body = GetComponent<Rigidbody>();
		animCtrl = GetComponent<Animator>();
	}
	
	protected virtual void Update()
	{
		if( !activated && IsWithinRangeOf( player,activateRange ) )
		{
			Activate();
		}
	}

	// transition from state1 to state2
	protected void Transition( string state1,string state2 )
	{
		animCtrl.SetBool( state1,false );
		animCtrl.SetBool( state2,true );
	}

	protected virtual void Activate()
	{
		activated = true;
	}

	protected bool IsWithinRangeOf( GameObject target,float range )
	{
		var dist = target.transform.position - transform.position;
		return ( dist.sqrMagnitude < Mathf.Pow( range,2 ) );
	}

	protected GameObject player;
	protected Rigidbody body;
	protected Animator animCtrl;

	[SerializeField] float activateRange = 15.0f;

	protected bool activated = false;
}
