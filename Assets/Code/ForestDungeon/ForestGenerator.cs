using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ForestGenerator
	:
	MonoBehaviour
{
	public void Generate()
	{
		int roomSize = PlayerPrefs.GetInt( "Room Size upgrade",0 ) * 2;
		roomWidth.Add( roomSize );
		roomHeight.Add( roomSize );

		int roomNumber = PlayerPrefs.GetInt( "Room Count upgrade",0 );
		roomCount.Add( roomNumber );

		int enemyCount = PlayerPrefs.GetInt( "Enemy Count upgrade",0 );
		nRoomEnemies.Add( enemyCount );

		int enemyvariety = PlayerPrefs.GetInt( "Enemy Variety upgrade",0 );
		unlockedEnemies = enemyvariety + 1;

		GenerateLayout();
	}

	void GenerateLayout()
	{
		var rooms = new List<RectI>();

		rooms.Add( new RectI( 0,0,roomWidth.Rand(),roomHeight.Rand() ) );

		int nCurRooms = roomCount.Rand();
		for( int i = 1; i < nCurRooms; ++i )
		{
			RectI curRoom = new RectI( 0,0,roomWidth.Rand(),roomHeight.Rand() );
			RectI oldRoom = rooms[i - 1];

			if( Random.Range( 0.0f,1.0f ) < 0.5f )
			{
				if( Random.Range( 0.0f,1.0f ) < 0.5f )
				{
					curRoom.x = Random.Range( oldRoom.x - curRoom.width - hallLen.min,oldRoom.x - curRoom.width - hallLen.max );
				}
				else
				{
					curRoom.x = Random.Range( oldRoom.x + oldRoom.width + hallLen.min,oldRoom.x + oldRoom.width + hallLen.max );
				}

				curRoom.y = Random.Range( oldRoom.y - curRoom.height + 1,oldRoom.y + oldRoom.height - 1 );
			}
			else
			{
				curRoom.x = Random.Range( oldRoom.x - curRoom.width + 1,oldRoom.x + oldRoom.width - 1 );

				if( Random.Range( 0.0f,1.0f ) < 0.5f )
				{
					curRoom.y = Random.Range( oldRoom.y - curRoom.height - hallLen.max,oldRoom.y - curRoom.height - hallLen.min );
				}
				else
				{
					curRoom.y = Random.Range( oldRoom.y + oldRoom.height + hallLen.min,oldRoom.y + oldRoom.height + hallLen.max );
				}
			}

			rooms.Add( curRoom );
		}

		int smallestX = 0;
		int smallestY = 0;
		foreach( var room in rooms )
		{
			if( room.x < smallestX ) smallestX = room.x;
			if( room.y < smallestY ) smallestY = room.y;
		}

		foreach( var room in rooms )
		{
			room.x -= ( smallestX - 1 );
			room.y -= ( smallestY - 1 );
		}

		int maxX = 0;
		int maxY = 0;
		foreach( var room in rooms )
		{
			if( room.x + room.width > maxX ) maxX = room.x + room.width;
			if( room.y + room.height > maxY ) maxY = room.y + room.height;
		}
		width = maxX + 1;
		height = maxY + 1;

		for( int y = 0; y < height; ++y )
		{
			for( int x = 0; x < width; ++x )
			{
				tilemap.Add( 1 );
			}
		}

		var halls = new List<Line>();
		for( int i = 0; i < rooms.Count - 1; ++i )
		{
			var curRoom = rooms[i];
			var nextRoom = rooms[i + 1];

			halls.Add( new Line( curRoom.GetRandPoint(),nextRoom.GetRandPoint() ) );
		}

		foreach( var hall in halls )
		{
			DrawHall( hall,0 );
		}

		// foreach( var room in rooms )
		int playerRoom = Random.Range( 0,rooms.Count );
		Vec2 playerPos = new Vec2( -1,-1 );
		for( int i = 0; i < rooms.Count; ++i )
		{
			var room = rooms[i];
			DrawRect( room.x,room.y,room.width,room.height,0 );

			if( i == playerRoom )
			{
				playerPos = room.GetRandPoint();
			}
			else
			{
				int nEnemies = nRoomEnemies.Rand();
				for( int j = 0; j < nEnemies; ++j )
				{
					int tries = 500;
					Vec2 pos;
					do
					{
						pos = room.GetRandPoint();
					}
					while( GetTile( pos.x,pos.y ) != 0 && --tries > 0 );
					if( tries > 1 ) SetTile( pos.x,pos.y,3 );
				}
			}
		}

		SetTile( playerPos.x,playerPos.y,2 );

		var floorObj = transform.GetChild( 0 );
		var floorScale = new Vector3( ( float )width * spacing,1.0f,( float )height * spacing );
		floorObj.transform.localScale = floorScale;
		floorObj.transform.position = new Vector3( floorScale.x / 2.0f,0.0f,floorScale.z / 2.0f );

		for( int y = 0; y < height; ++y )
		{
			for( int x = 0; x < width; ++x )
			{
				var tile = GetTile( x,y );
				var worldPos = new Vector3( ( float )x * spacing,0.0f,( float )y * spacing );
				switch( tile )
				{
					// 0 = empty
					case 1: // wall
						if( CheckSurroundingTiles( x,y,3 ) > 0 )
						{
							var wall = Instantiate( wallPrefab,transform );
							wall.transform.position = worldPos;
						}
						break;
					case 2: // player
						{
							var player = Instantiate( playerPrefabs[0] );
							player.transform.position = worldPos;
							FindObjectOfType<NewPlayerCam>().SetPlayer( player );
						}
						break;
					case 3: // enemies
						{
							var enemy = Instantiate( enemyPrefabs[Random.Range( 0,unlockedEnemies )] );
							enemy.transform.position = worldPos;
						}
						break;
				}
			}
		}
	}

	void DrawRect( int x,int y,int width,int height,int val )
	{
		if( width < 0 )
		{
			x = x + width;
			width *= -1;
		}
		if( height < 0 )
		{
			y = y + height;
			height *= -1;
		}
		
		for( int ry = y; ry < y + height; ++ry )
		{
			for( int rx = x; rx < x + width; ++rx )
			{
				SetTile( rx,ry,val );
			}
		}
	}

	void DrawLine( int x1,int y1,int x2,int y2,int val )
	{
		Assert.IsTrue( x1 == x2 || y1 == y2 );

		if( x2 < x1 )
		{
			var temp = x1;
			x1 = x2;
			x2 = temp;
		}
		if( y2 < y1 )
		{
			var temp = y1;
			y1 = y2;
			y2 = temp;
		}

		for( int ry = y1; ry <= y2; ++ry )
		{
			for( int rx = x1; rx <= x2; ++rx )
			{
				SetTile( rx,ry,val );
			}
		}
	}

	void DrawHall( Line hall,int val )
	{
		if( Random.Range( 0.0f,1.0f ) < 0.5f )
		{
			// DrawRect( hall.start.x,hall.start.y,1,hall.GetHeight(),0 );
			// DrawRect( hall.start.x,hall.end.y,hall.GetWidth(),1,0 );
			DrawLine( hall.start.x,hall.start.y,hall.start.x,hall.end.y,0 );
			DrawLine( hall.start.x,hall.end.y,hall.end.x,hall.end.y,0 );
		}
		else
		{
			// DrawRect( hall.start.x,hall.start.y,hall.GetWidth(),1,0 );
			// DrawRect( hall.end.x,hall.start.y,1,hall.GetHeight(),0 );
			DrawLine( hall.start.x,hall.start.y,hall.end.x,hall.start.y,0 );
			DrawLine( hall.end.x,hall.start.y,hall.end.x,hall.end.y,0 );
		}
	}

	void SetTile( int x,int y,int val )
	{
		tilemap[y * width + x] = val;
	}

	int GetTile( int x,int y )
	{
		if( x < 0 || x >= width || y < 0 || y >= height ) return( 1 );

		return( tilemap[y * width + x] );
	}

	int CheckSurroundingTiles( int x,int y,int range )
	{
		int count = 0;

		for( int cy = y - range / 2; cy <= y + range / 2; ++cy )
		{
			for( int cx = x - range / 2; cx <= x + range / 2; ++cx )
			{
				if( GetTile( cx,cy ) != 1 ) ++count;
			}
		}

		return( count );
	}

	// In world space.
	Vector3 GetRandSpawnPos()
	{
		Vector3 randPos = Vector3.zero;
		do
		{
			randPos.x = Random.Range( 0,width );
			randPos.z = Random.Range( 0,height );
		}
		while( GetTile( ( int )randPos.x,( int )randPos.z ) != 0 );

		randPos *= spacing;
		randPos.y = 1.0f;

		return( randPos );
	}

	List<int> tilemap = new List<int>();
	int width;
	int height;

	// enable to use serializefield values
	// [SerializeField] bool serializeFieldOverride = false;

	[SerializeField] RangeI roomWidth = new RangeI( 3,5 );
	[SerializeField] RangeI roomHeight = new RangeI( 3,5 );

	[SerializeField] RangeI hallLen = new RangeI( 3,7 );

	[SerializeField] RangeI roomCount = new RangeI( 3,5 );

	[SerializeField] GameObject wallPrefab = null;
	[SerializeField] float spacing = 1.0f;

	[SerializeField] List<GameObject> playerPrefabs = new List<GameObject>();

	[SerializeField] RangeI nRoomEnemies = new RangeI( 1,3 );
	[SerializeField] int unlockedEnemies = 1;
	[SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
}
