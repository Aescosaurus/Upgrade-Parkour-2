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

		animCtrl = GetComponent<Animator>();
		charCtrl = GetComponent<CharacterController>();
	}

	protected override void Update()
	{
		base.Update();

		yVel -= gravAcc * Time.deltaTime;
		charCtrl.Move( transform.up * yVel );

		if( charCtrl.isGrounded ) yVel = 0.0f;
	}

	protected void Move( Vector3 dir )
	{
		animCtrl.SetBool( "walk",true );

		var rot = transform.eulerAngles;
		rot.y = Mathf.Atan2( dir.x,dir.z ) * Mathf.Rad2Deg;
		rot.y = Mathf.LerpAngle( transform.eulerAngles.y,rot.y,rotSpeed * Time.deltaTime );
		transform.eulerAngles = rot;

		charCtrl.Move( dir.normalized * moveSpeed * Time.deltaTime );
	}

	protected void StopMoving()
	{
		animCtrl.SetBool( "walk",false );
	}

	protected void Look( Vector3 dir )
	{
		dir.y = 0.0f;
		if( dir.sqrMagnitude > 0.0f )
		{
			transform.forward = dir;
		}
	}

	Animator animCtrl;
	CharacterController charCtrl;

	[SerializeField] float moveSpeed = 1.0f;
	[SerializeField] float rotSpeed = 2.4f;
	[SerializeField] float gravAcc = 0.3f;
	float yVel = 0.0f;
}
