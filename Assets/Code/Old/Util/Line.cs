using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
	public Line( Vec2 start,Vec2 end )
	{
		this.start = start;
		this.end = end;
	}

	public int GetWidth()
	{
		return( end.x - start.x );
	}

	public int GetHeight()
	{
		return( end.y - start.y );
	}

	public Vec2 start;
	public Vec2 end;
}
