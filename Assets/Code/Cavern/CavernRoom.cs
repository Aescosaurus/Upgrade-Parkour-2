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
			rightExits.Add( new List<int>{ 8,3,1,4 } );
			rightExits.Add( new List<int>{ 10,7,2,0,1,4 } );
			rightExits.Add( new List<int>{ 10,7,2,0,3,6 } );

			doorMap.Add( 0,upExits );
			doorMap.Add( 2,leftExits );
			doorMap.Add( 3,rightExits );
		}

		if( decoConns == null )
		{
			decoConns = new Dictionary<int,List<int>>();

			int curConn = 0;
			decoConns.Add( curConn++,new List<int>(){ 2,0 } );
			decoConns.Add( curConn++,new List<int>(){ 0,3,1 } );
			decoConns.Add( curConn++,new List<int>(){ 1,4 } );
			decoConns.Add( curConn++,new List<int>(){ 2,5,7 } );
			decoConns.Add( curConn++,new List<int>(){ 3,5,8,6 } );
			decoConns.Add( curConn++,new List<int>(){ 4,6,9 } );
			decoConns.Add( curConn++,new List<int>(){ 7,10 } );
			decoConns.Add( curConn++,new List<int>(){ 10,8,11 } );
			decoConns.Add( curConn++,new List<int>(){ 11,9 } );
		}

		// var exitDir = 1;
		while( exitDir == 1 ) exitDir = Random.Range( 0,3 + 1 );

		if( doorMap.ContainsKey( exitDir ) )
		{
			var children = new List<Transform>();
			var walls = transform.Find( "Walls" );
			for( var i = 0; i < walls.childCount; ++i ) children.Add( walls.GetChild( i ) );

			exitChoice = Random.Range( 0,doorMap[exitDir].Count );
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

	public void PopulateRoom( List<GameObject> decoPrefabs,float decoSpawnChance,
		List<GameObject> enemyPrefabs )
	{
		var possibleSpawnAreas = new List<Transform>();

		Transform spawnAreas = transform.Find( "SpawnAreas" );
		for( int i = 0; i < spawnAreas.childCount; ++i )
		{
			bool canSpawn = false;
			foreach( int decoWall in decoConns[i] )
			{
				foreach( int openWall in doorMap[exitDir][exitChoice] )
				{
					if( decoWall == openWall ) canSpawn = true;
				}
			}

			if( canSpawn ) possibleSpawnAreas.Add( spawnAreas.GetChild( i ) );
		}

		foreach( var area in possibleSpawnAreas )
		{
			var boxes = area.GetComponentsInChildren<BoxCollider>();
			var chosenBox = boxes[Random.Range( 0,boxes.Length - 1 )];
			var spawnLoc = BoxPointSelector.GetRandPointWithinBox(
				chosenBox,1.6f );

			GameObject spawnedObj = null;

			if( Random.Range( 0.0f,1.0f ) < decoSpawnChance )
			{
				spawnedObj = Instantiate( decoPrefabs[Random.Range( 0,decoPrefabs.Count )] );
				spawnLoc.y = area.position.y;
			}
			else
			{
				spawnedObj = Instantiate( enemyPrefabs[Random.Range( 0,enemyPrefabs.Count )] );
			}
			spawnedObj.transform.position = spawnLoc;
			spawnedObj.transform.Rotate( Vector3.up,Random.Range( 0.0f,360.0f ) );
		}

		// int nChildren = spawnAreas.childCount;
		// for( int i = 0; i < nChildren; ++i )
		// {
		// 	var boxes = spawnAreas.GetChild( i ).GetComponentsInChildren<BoxCollider>();
		// 	foreach( var box in boxes ) Destroy( box );
		// }
	}

	// walls have a chance of breaking anyway

	int exitDir = 1;
	int exitChoice = 0;

	static Dictionary<int,List<List<int>>> doorMap = null;
	static Dictionary<int,List<int>> decoConns = null;
}
