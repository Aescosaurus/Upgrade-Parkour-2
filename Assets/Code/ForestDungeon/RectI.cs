using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectI
{
	public RectI( int x,int y,int width,int height )
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	public bool ContainsPoint( int x,int y )
	{
		return( x > this.x && x < this.x + this.width &&
			y > this.y && y < this.y + this.height );
	}

	public Vec2 GetRandPoint()
	{
		return( new Vec2( Random.Range( x,x + width ),Random.Range( y,y + height ) ) );
	}

	public int x;
	public int y;
	public int width;
	public int height;
}
