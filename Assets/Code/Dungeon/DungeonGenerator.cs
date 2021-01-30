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
		corridorPrefabs.Add( Resources.Load<GameObject>( "Prefabs/Dungeon/DungeonDeadEnd" ) );
		corridorPrefabs.Add( Resources.Load<GameObject>( "Prefabs/Dungeon/DungeonI" ) );
		corridorPrefabs.Add( Resources.Load<GameObject>( "Prefabs/Dungeon/DungeonL" ) );
		corridorPrefabs.Add( Resources.Load<GameObject>( "Prefabs/Dungeon/DungeonT" ) );
		corridorPrefabs.Add( Resources.Load<GameObject>( "Prefabs/Dungeon/DungeonX" ) );

		wallPrefab = Resources.Load<GameObject>( "Prefabs/Dungeon/DungeonGateWall" );

		layout = GenerateLayout( dungeonSize,dungeonSize,( int )( ( float )dungeonSize * roomPercent ) );

		for( int y = 0; y < dungeonSize; ++y )
		{
			for( int x = 0; x < dungeonSize; ++x )
			{
				if( CheckRoom( x,y ) ) GenCorridor( x,y );
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

	void GenCorridor( int x,int y )
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
		curCorridor.transform.position += new Vector3( x,0,y ) * corridorSize;
		// curCorridor.transform.position += Vector3.left * dungeonSize / 2 + Vector3.forward * dungeonSize / 2;
		curCorridor.transform.Rotate( Vector3.up,rot );
		PopulateCorridor( curCorridor,x != 0 || y != 0 );
	}

	void PopulateCorridor( GameObject corridor,bool spawnEnemies = true )
	{
		var possibleSpawnAreas = corridor.GetComponentsInChildren<BoxCollider>();
		var spawnAreas = new List<BoxCollider>();
		foreach( var area in possibleSpawnAreas )
		{
			if( area.isTrigger ) spawnAreas.Add( area );
		}

		if( spawnEnemies )
		{
			var nEnemies = nRoomEnemies.Rand();

			for( int i = 0; i < nEnemies; ++i )
			{
				var enemy = Instantiate( enemyPrefabs[Random.Range( 0,enemyPrefabs.Count - 1 )] );
				enemy.transform.position = BoxPointSelector.GetRandPointWithinBox(
					spawnAreas[Random.Range( 0,spawnAreas.Count - 1 )] );
			}
		}

		foreach( var area in spawnAreas ) Destroy( area );

		var walls = corridor.transform.Find( "WallLocs" );
		for( int i = 0; i < walls.childCount; ++i )
		{
			if( Random.Range( 0.0f,1.0f ) < wallChance ) Instantiate( wallPrefab,walls.GetChild( i ) );
		}
	}

	bool CheckRoom( int x,int y )
	{
		if( x < 0 || x >= dungeonSize || y < 0 || y >= dungeonSize ) return( false );
		return( layout[y * dungeonSize + x] );
	}

	List<GameObject> corridorPrefabs = new List<GameObject>();

	[SerializeField] int dungeonSize = 10;
	[SerializeField] float roomPercent = 0.5f;

	List<bool> layout;

	[SerializeField] float corridorSize = 10.0f;
	[SerializeField] RangeI nRoomEnemies = new RangeI( 0,4 );

	[SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
	GameObject wallPrefab;

	[SerializeField] float wallChance = 0.6f;
}
