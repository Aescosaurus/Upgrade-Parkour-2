﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

public class DungeonGenerator
	:
	MonoBehaviour
{
	void Start()
	{
		hubPortalPrefab = ResLoader.Load( "Prefabs/HubPortal" );
		stairsPrefab = ResLoader.Load( "Prefabs/StairsPortal" );

		corridorPrefabs.Add( ResLoader.Load( "Prefabs/Dungeon/DungeonDeadEnd" ) );
		corridorPrefabs.Add( ResLoader.Load( "Prefabs/Dungeon/DungeonI" ) );
		corridorPrefabs.Add( ResLoader.Load( "Prefabs/Dungeon/DungeonL" ) );
		corridorPrefabs.Add( ResLoader.Load( "Prefabs/Dungeon/DungeonT" ) );
		corridorPrefabs.Add( ResLoader.Load( "Prefabs/Dungeon/DungeonX" ) );
		bossRoomPrefab = ResLoader.Load( "Prefabs/Dungeon/DungeonBossRoom" );

		wallPrefab = ResLoader.Load( "Prefabs/Dungeon/DungeonGate2" );

		if( PlayerPrefs.GetInt( "curfloor",0 ) >= bossFloor )
		{
			Instantiate( bossRoomPrefab,transform );
		}
		else
		{
			var curRoomCount = ( int )( ( float )dungeonSize * roomPercent );
			int curNRoom = 0;
			layout = GenerateLayout( dungeonSize,dungeonSize,curRoomCount );

			for( int y = 0; y < dungeonSize; ++y )
			{
				for( int x = 0; x < dungeonSize; ++x )
				{
					if( CheckRoom( x,y ) ) GenCorridor( x,y,++curNRoom >= curRoomCount );
				}
			}
		}
	}

	List<bool> GenerateLayout( int width,int height,int nRooms )
	{
		var layout = new List<bool>();
		layout.AddRange( Enumerable.Repeat( false,width * height ) );

		var start = Vector2.zero;
		while( nRooms > 0 )
		{
			layout[( int )start.y * width + ( int )start.x] = true;

			do
			{
				var xMove = Random.Range( -1,2 );
				if( xMove != 0 ) start.x += xMove;
				else start.y += Random.Range( -1,2 );

				if( start.x < 0 ) start.x = 0;
				if( start.x >= width ) start.x = width - 1;
				if( start.y < 0 ) start.y = 0;
				if( start.y >= height ) start.y = height - 1;
			}
			while( layout[( int )start.y * width + ( int )start.x] );

			--nRooms;
		}

		return( layout );
	}

	void GenCorridor( int x,int y,bool spawnExit )
	{
		var dirs = new List<bool>();
		dirs.Add( CheckRoom( x,y - 1 ) );
		dirs.Add( CheckRoom( x,y + 1 ) );
		dirs.Add( CheckRoom( x - 1,y ) );
		dirs.Add( CheckRoom( x + 1,y ) );
		int nAdjacent = 0;
		foreach( var dir in dirs ) if( dir ) ++nAdjacent;

		int inst = -1;
		float rot = 0.0f;
		if( nAdjacent == 4 ) inst = 4;
		else if( nAdjacent == 3 )
		{
			inst = 3;
			if( dirs[0] && dirs[1] && dirs[2] ) rot = -90.0f;
			else if( dirs[0] && dirs[2] && dirs[3] ) rot = 180.0f;
			else if( dirs[0] && dirs[1] && dirs[3] ) rot = 90.0f;
		}
		else if( nAdjacent == 2 )
		{
			if( dirs[0] && dirs[1] ) inst = 1;
			else if( dirs[2] && dirs[3] )
			{
				inst = 1;
				rot = 90.0f;
			}
			else if( dirs[0] && dirs[3] )
			{
				inst = 2;
				rot = 90.0f;
			}
			else if( dirs[0] && dirs[2] )
			{
				inst = 2;
				rot = 180.0f;
			}
			else if( dirs[2] && dirs[1] )
			{
				inst = 2;
				rot = -90.0f;
			}
			else if( dirs[1] && dirs[3] )
			{
				inst = 2;
				rot = 0.0f;
			}
		}
		else if( nAdjacent == 1 )
		{
			inst = 0;
			if( dirs[1] ) rot = 180.0f;
			else if( dirs[2] ) rot = 90.0f;
			else if( dirs[3] ) rot = -90.0f;
		}

		Assert.IsTrue( inst >= 0 );
		var curCorridor = Instantiate( corridorPrefabs[inst],transform );
		curCorridor.transform.position += new Vector3( x,0,y ) * corridorSize + new Vector3(
				Random.Range( -0.0001f,0.0001f ),
				Random.Range( -0.0001f,0.0001f ),
				Random.Range( -0.0001f,0.0001f ) );
		// curCorridor.transform.position += Vector3.left * dungeonSize / 2 + Vector3.forward * dungeonSize / 2;
		curCorridor.transform.Rotate( Vector3.up,rot );
		PopulateCorridor( curCorridor,x != 0 || y != 0,spawnExit );
	}

	void PopulateCorridor( GameObject corridor,bool spawnEnemies = true,bool spawnExit = false )
	{
		var walls = corridor.transform.Find( "WallLocs" );
		for( int i = 0; i < walls.childCount; ++i )
		{
			if( Random.Range( 0.0f,1.0f ) < wallChance || !spawnEnemies || spawnExit ) Instantiate( wallPrefab,walls.GetChild( i ) );
		}

		// var possibleSpawnAreas = corridor.GetComponentsInChildren<BoxCollider>();
		// var spawnAreas = new List<BoxCollider>();
		// foreach( var area in possibleSpawnAreas )
		// {
		// 	if( area.isTrigger ) spawnAreas.Add( area );
		// }
		var enemySpawnAreas = corridor.transform.Find( "EnemySpawnArea" ).GetComponents<BoxCollider>().ToList();
		var decoSpawnAreas = corridor.transform.Find( "DecoSpawnArea" ).GetComponents<BoxCollider>().ToList();

		var prevSpawned = new List<GameObject>();

		if( spawnExit )
		{
			prevSpawned.Add( TrySpawnPrefab( hubPortalPrefab,enemySpawnAreas,prevSpawned ) );
			prevSpawned.Add( TrySpawnPrefab( stairsPrefab,enemySpawnAreas,prevSpawned ) );
		}
		else if( spawnEnemies )
		{
			var nEnemies = nRoomEnemies.Rand();
			for( int i = 0; i < nEnemies; ++i )
			{
				prevSpawned.Add( TrySpawnPrefab( SelectEnemyPrefab(),enemySpawnAreas,prevSpawned ) );
			}
		}

		int roomDecoCount = nDecorations.Rand();
		for( int i = 0; i < roomDecoCount; ++i )
		{
			prevSpawned.Add( TrySpawnPrefab( decorations[Random.Range( 0,decorations.Count )],decoSpawnAreas,prevSpawned ) );
		}
		
		foreach( var area in enemySpawnAreas ) Destroy( area );
		foreach( var area in decoSpawnAreas ) Destroy( area );
	}

	// void TrySpawnPrefab( GameObject prefab,List<BoxCollider> spawnAreas,bool randY = true )
	// {
	// 	var obj = Instantiate( prefab );
	// 	obj.transform.position = BoxPointSelector.GetRandPointWithinBox(
	// 		spawnAreas[Random.Range( 0,spawnAreas.Count )] );
	// 	if( !randY ) obj.transform.position += Vector3.down * obj.transform.position.y;
	// }
	GameObject TrySpawnPrefab( GameObject prefab,List<BoxCollider> spawnAreas,List<GameObject> prevSpawned )
	{
		var obj = Instantiate( prefab );
		int tries = 50;
		do
		{
			obj.transform.position = BoxPointSelector.GetRandPointWithinBox(
				spawnAreas[Random.Range( 0,spawnAreas.Count )],spawnSpacing );
			obj.transform.position += Vector3.down * obj.transform.position.y;
		}
		while( CheckOverlapping( obj,prevSpawned ) && --tries > 0 );

		obj.transform.Rotate( Vector3.up,Random.Range( 0.0f,360.0f ) );
		
		return( obj );
	}

	// Return true if there is overlapping within prevSpawned.
	bool CheckOverlapping( GameObject curObj,List<GameObject> prevSpawned )
	{
		foreach( var obj in prevSpawned )
		{
			if( obj != null )
			{
				var diff = curObj.transform.position - obj.transform.position;
				if( diff.sqrMagnitude < spawnSpacing * spawnSpacing )
				{
					return ( true );
				}
			}
		}

		return( false );
	}

	bool CheckRoom( int x,int y )
	{
		if( x < 0 || x >= dungeonSize || y < 0 || y >= dungeonSize ) return( false );
		return( layout[y * dungeonSize + x] );
	}

	GameObject SelectEnemyPrefab()
	{
		return( enemyPrefabs[Random.Range( 0,enemyPrefabs.Count )] );
	}

	GameObject hubPortalPrefab;
	GameObject stairsPrefab;

	List<GameObject> corridorPrefabs = new List<GameObject>();
	GameObject bossRoomPrefab;

	[SerializeField] int dungeonSize = 10;
	[SerializeField] float roomPercent = 0.5f;

	[SerializeField] int bossFloor = 5;

	List<bool> layout;

	[SerializeField] float corridorSize = 10.0f;
	[SerializeField] RangeI nRoomEnemies = new RangeI( 0,4 );

	[SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
	GameObject wallPrefab;

	[SerializeField] float wallChance = 0.6f;
	[SerializeField] RangeI nDecorations = new RangeI( 3,5 );

	[SerializeField] float spawnSpacing = 1.8f;
	[SerializeField] List<GameObject> decorations = new List<GameObject>();
}
