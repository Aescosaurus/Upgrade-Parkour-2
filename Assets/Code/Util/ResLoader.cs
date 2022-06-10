using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Caches prefabs so you don't have to resources.load every time.
public class ResLoader
{
	public static GameObject Load( string path )
	{
		if( !codex.ContainsKey( path ) )
		{
			codex.Add( path,Resources.Load<GameObject>( path ) );
		}

		return( codex[path] );
	}

	static Dictionary<string,GameObject> codex = new Dictionary<string,GameObject>();
}
