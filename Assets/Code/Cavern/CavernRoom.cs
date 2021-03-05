using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernRoom
	:
	MonoBehaviour
{
	public Transform Generate( bool allowExit )
	{
		if( doorMap == null )
		{
			doorMap = new Dictionary<int,List<List<int>>>();

			var upExits = new List<List<int>>();
			upExits.Add( new List<int>{ 3,8 } );
			upExits.Add( new List<int>{ 11,9,4,1 } );
			upExits.Add( new List<int>{ 10,7,2,0 } );
			upExits.Add( new List<int>{ 10,7,5,6,4,1 } );
			upExits.Add( new List<int>{ 11,9,6,5,2,0 } );
			upExits.Add( new List<int>{ 8,6,4,1 } );
			upExits.Add( new List<int>{ 8,5,2,0 } );
			upExits.Add( new List<int>{ 11,9,6,3 } );
			upExits.Add( new List<int>{ 10,7,5,3 } );
			
			var leftExits = new List<List<int>>();
			leftExits.Add( new List<int>{ 8,5 } );
			leftExits.Add( new List<int>{ 10,7 } );
			leftExits.Add( new List<int>{ 8,3,0,2 } );
			leftExits.Add( new List<int>{ 11,9,6,5 } );
			leftExits.Add( new List<int>{ 11,9,4,1,0,2 } );
			leftExits.Add( new List<int>{ 11,9,4,1,3,5 } );

			var rightExits = new List<List<int>>();
			rightExits.Add( new List<int>{ 11,9 } );
			rightExits.Add( new List<int>{ 8,6 } );
			rightExits.Add( new List<int>{ 8,3,1 } );
			rightExits.Add( new List<int>{ 10,7,2,0,1,4 } );
			rightExits.Add( new List<int>{ 10,7,2,0,3,6 } );

			doorMap.Add( 0,upExits );
			doorMap.Add( 2,leftExits );
			doorMap.Add( 3,rightExits );
		}

		if( spawnConnections == null )
		{
			spawnConnections = new Dictionary<int,List<int>>();

			int curConn = 0;
			spawnConnections.Add( curConn++,new List<int>(){ 11,9 } );
			spawnConnections.Add( curConn++,new List<int>(){ 11,8,10 } );
			spawnConnections.Add( curConn++,new List<int>(){ 10,7 } );
			spawnConnections.Add( curConn++,new List<int>(){ 8,6,3,5 } );
			spawnConnections.Add( curConn++,new List<int>(){ 7,5,2 } );
			spawnConnections.Add( curConn++,new List<int>(){ 4,1 } );
			spawnConnections.Add( curConn++,new List<int>(){ 1,3,0 } );
			spawnConnections.Add( curConn++,new List<int>(){ 0,2 } );
		}

		var exitDir = 1;
		while( exitDir == 1 ) exitDir = Random.Range( 0,3 + 1 );

		if( doorMap.ContainsKey( exitDir ) )
		{
			var children = new List<Transform>();
			var walls = transform.Find( "Walls" );
			for( var i = 0; i < walls.childCount; ++i ) children.Add( walls.GetChild( i ) );

			var exitChoice = Random.Range( 0,doorMap[exitDir].Count );
			// print( exitDir + " " + exitChoice + " " + doorMap[exitDir].Count );
			foreach( var wall in doorMap[exitDir][exitChoice] )
			{
				Destroy( children[wall].gameObject );
			}
		}
		
		var doors = transform.Find( "Doors" );
		var exitDoor = doors.GetChild( exitDir );
		var startDoor = doors.GetChild( 1 );
		// if( exitDir != 1 ) Destroy( doors.GetChild( exitDir ).gameObject );
		Destroy( startDoor.gameObject );

		// multiple exit dirs

		if( exitDir == 1 ) exitDoor = null;

		return( exitDoor );
	}

	// walls have a chance of breaking anyway

	static Dictionary<int,List<List<int>>> doorMap = null;
	static Dictionary<int,List<int>> spawnConnections = null;
}
