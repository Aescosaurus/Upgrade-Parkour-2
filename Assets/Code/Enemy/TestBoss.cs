using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss
	:
	BossBase
{
	protected override void Update()
	{
		base.Update();

		if( shotTimer.Update( Time.deltaTime ) )
		{
			shotTimer.Reset();
			var targetPos = player.transform.position;
			if( Random.Range( 0.0f,1.0f ) < 0.5f ) targetPos += player.GetComponent<PlayerWalk>().GetVel() * predictFactor;
			var shotDir = targetPos - transform.position;

			if( Random.Range( 0.0f,1.0f ) < 0.9f ) Fire( shotDir );
			if( Random.Range( 0.0f,1.0f ) < 0.3f ) Lob( player.transform.position );
			if( Random.Range( 0.0f,1.0f ) < 0.6f ) SpawnBopper( shotDir );
		}
	}

	[SerializeField] Timer shotTimer = new Timer( 0.5f );
	// [SerializeField] int shotCount = 10;
	// [Range( 0.0f,Mathf.PI )]
	// [SerializeField] float spacing = Mathf.PI / 10.0f;
	// [Range( 0.0f,Mathf.PI )]
	// [SerializeField] float ySpacing = Mathf.PI / 5.0f;
	[SerializeField] float predictFactor = 5.0f;
}
