using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBoss
	:
	BossBase
{
	void Update()
	{
		if( shotTimer.Update( Time.deltaTime ) )
		{
			shotTimer.Reset();
			var shotDir = player.transform.position - transform.position;
			var shotAng = Mathf.Atan2( shotDir.z,shotDir.x );
			// for( int i = 0; i < shotCount; ++i )
			// {
			// 	Fire( shotDir + new Vector3(
			// 		Random.Range( -1.0f,1.0f ),
			// 		Random.Range( -1.0f,1.0f ),
			// 		Random.Range( -1.0f,1.0f )
			// 		) );
			// }

			Fire( shotDir );

			for( int i = -shotCount / 2; i < shotCount / 2; ++i )
			{
				for( float yAng = -Mathf.PI / 5.0f; yAng < Mathf.PI / 5.0f; yAng += spacing )
				{
					if( Random.Range( 0.0f,1.0f ) < 0.6f )
					{
						Fire( new Vector3( Mathf.Cos( shotAng + i * spacing ),yAng,Mathf.Sin( shotAng + i * spacing ) ) );
					}
				}
			}
		}
	}

	[SerializeField] Timer shotTimer = new Timer( 0.5f );
	[SerializeField] int shotCount = 10;
	[Range( 0.0f,Mathf.PI )]
	[SerializeField] float spacing = Mathf.PI / 10.0f;
}
