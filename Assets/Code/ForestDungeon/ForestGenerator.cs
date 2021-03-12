using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator
	:
	MonoBehaviour
{
	void Start()
	{
		Generate();
	}

	void Generate()
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

		foreach( var room in rooms )
		{
			DrawRect( room.x,room.y,room.width,room.height,0 );
		}
		
		foreach( var hall in halls )
		{
			DrawHall( hall,0 );
		}

		for( int y = 0; y < height; ++y )
		{
			for( int x = 0; x < width; ++x )
			{
				if( GetTile( x,y ) == 1 )
				{
					var wall = Instantiate( wallPrefab );
					wall.transform.position = new Vector3( ( float )x * spacing,0.0f,( float )y * spacing );
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

	void DrawHall( Line hall,int val )
	{
		if( Random.Range( 0.0f,1.0f ) < 0.5f )
		{
			DrawRect( hall.start.x,hall.start.y,1,hall.GetHeight(),0 );
			DrawRect( hall.start.x,hall.end.y,hall.GetWidth(),1,0 );
		}
		else
		{
			DrawRect( hall.start.x,hall.start.y,hall.GetWidth(),1,0 );
			DrawRect( hall.end.x,hall.start.y,1,hall.GetHeight(),0 );
		}
	}

	void SetTile( int x,int y,int val )
	{
		tilemap[y * width + x] = val;
	}

	int GetTile( int x,int y )
	{
		return( tilemap[y * width + x] );
	}

	List<int> tilemap = new List<int>();
	int width;
	int height;

	[SerializeField] RangeI roomWidth = new RangeI( 3,5 );
	[SerializeField] RangeI roomHeight = new RangeI( 3,5 );

	[SerializeField] RangeI hallLen = new RangeI( 3,7 );

	[SerializeField] RangeI roomCount = new RangeI( 3,5 );

	[SerializeField] GameObject wallPrefab = null;
	[SerializeField] float spacing = 1.0f;
}
