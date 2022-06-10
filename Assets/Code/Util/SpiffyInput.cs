using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiffyInput
	:
	MonoBehaviour
{
	public static bool CheckAxis( string axis,bool ignorePause = false )
	{
		if( !ignorePause && PauseMenu.IsOpen() ) return( false );

		if( !canPress.ContainsKey( axis ) ) canPress.Add( axis,false );

		bool pressing = Input.GetAxis( axis ) > 0.0f;

		if( canPress[axis] )
		{
			if( pressing )
			{
				canPress[axis] = false;
				return( true );
			}
			else return( false );
		}
		else
		{
			if( !pressing ) canPress[axis] = true;
			return( false );
		}
	}

	// Lets you repeat keys but still checks pause menu open.
	public static bool CheckFree( string axis,bool ignorePause = false )
	{
		if( !ignorePause && PauseMenu.IsOpen() ) return( false );
		else return( Input.GetAxis( axis ) > 0.0f );
	}

	// Input.GetAxis but gives 0 when paused.
	public static float GetAxis( string axis,bool ignorePause = false )
	{
		if( !ignorePause && PauseMenu.IsOpen() ) return( 0.0f );
		else return ( Input.GetAxis( axis ) );
	}

	static Dictionary<string,bool> canPress = new Dictionary<string,bool>();
}
