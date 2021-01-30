using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangeF
{
	public RangeF( float min,float max )
	{
		this.min = min;
		this.max = max;
	}

	public float Rand()
	{
		return ( Random.Range( min,max ) );
	}

	[SerializeField] public float min;
	[SerializeField] public float max;
}


[System.Serializable]
public class RangeI
{
	public RangeI( int min,int max )
	{
		this.min = min;
		this.max = max;
	}

	public int Rand()
	{
		return( Random.Range( min,max + 1 ) );
	}

	[SerializeField] public int min;
	[SerializeField] public int max;
}
