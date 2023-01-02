using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkRocket
	:
	ToolBase
{
	void Start()
	{
		cam = Camera.main;

		Reload();
	}

	void Update()
	{
		refire.Update( Time.deltaTime );
		if( SpiffyInput.CheckFree( inputKey ) )
		{
			if( refire.IsDone() )
			{
				flyDir = cam.transform.forward;
				refire.Reset();
				flyDur.Reset();
				flying = true;
			}
		}
	}

	public override void Reload()
	{
		refire.Update( refire.GetDuration() );
		// update indicators
	}

	Camera cam;
	Vector3 flyDir = Vector3.zero;
	bool flying = false;

	[SerializeField] Timer refire = new Timer( 5.0f );
	[SerializeField] Timer flyDur = new Timer( 1.0f );
}
