using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkRocket
	:
	ToolBase
{
	void Start()
	{
		arrowSpot = Camera.main.transform.Find( "ArrowSpot" );
		// load arrow prefab
		curArrow = Instantiate( arrowPrefab,arrowSpot );
		curArrow.SetActive( false );

		Reload();
	}

	void Update()
	{
		refire.Update( Time.deltaTime );
		if( SpiffyInput.CheckFree( inputKey ) )
		{
			if( refire.IsDone() )
			{
				if( !aiming )
				{
					aiming = true;
					curArrow.SetActive( true );
				}

				var moveDir = new Vector2( SpiffyInput.GetAxis( "Horizontal" ),
					SpiffyInput.GetAxis( "Vertical" ) ).normalized;

				curArrow.transform.Rotate( Vector3.right,moveDir.x * arrowSensitivity.x );
				curArrow.transform.Rotate( Vector3.up,moveDir.y * arrowSensitivity.y );
			}
		}
		else if( aiming )
		{
			aiming = false;
			refire.Reset();
			curArrow.SetActive( false );
			// continually apply force in direction
		}
	}

	public override void Reload()
	{
		refire.Update( refire.GetDuration() );
		// update indicators
	}

	Transform arrowSpot;
	GameObject arrowPrefab;

	// string attaching launcher to rocket

	[SerializeField] Timer refire = new Timer( 1.0f );
	[SerializeField] Vector2 arrowSensitivity = Vector2.one * 0.1f;

	GameObject curArrow = null;
	bool aiming = false;
}
