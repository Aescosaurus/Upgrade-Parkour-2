using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBipedBase
	:
	EnemyBase
{
	protected override void Start()
	{
		base.Start();

		// animCtrl = GetComponent<Animator>();
		// charCtrl = GetComponent<CharacterController>();
	}

	protected override void Update()
	{
		base.Update();

		// yVel -= gravAcc * Time.deltaTime;
		// charCtrl.Move( transform.up * yVel );

		// if( charCtrl.isGrounded ) yVel = 0.0f;
	}

	protected override void Move( Vector3 dir )
	{
		base.Move( dir );

		animCtrl.SetBool( "walk",true );

		// charCtrl.Move( dir.normalized * moveSpeed * Time.deltaTime );
	}

	protected override void StopMoving()
	{
		base.StopMoving();

		animCtrl.SetBool( "walk",false );
	}

	// Animator animCtrl;
	// CharacterController charCtrl;

	// [SerializeField] float gravAcc = 0.3f;
	// float yVel = 0.0f;
}
