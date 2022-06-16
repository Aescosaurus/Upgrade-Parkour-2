using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PartHand
{
	public enum PartType
	{
		None,
		ExplodeBarrel,
		Count
	}

	static void LoadAllParts()
	{
		Assert.IsTrue( !loadedParts );

		for( int i = 0; i < ( int )PartType.Count; ++i ) partPrefabs.Add( null );

		LoadPart( PartType.ExplodeBarrel );
	}

	public static void SpawnParts( Vector3 loc,int amount,PartType type )
	{
		Assert.IsTrue( type != PartType.Count );
		if( !loadedParts ) LoadAllParts();
	
		if( type != PartType.None )
		{
			var curPartObj = GameObject.Instantiate( partPrefabs[( int )type],loc,Quaternion.identity );
			// curPartObj.transform.position = loc;
	
			var partSys = curPartObj.GetComponent<ParticleSystem>();
			partSys.Emit( amount );
			GameObject.Destroy( curPartObj,partSys.main.duration );
		}
	}
	
	public static void SpawnFollowingParts( Transform follow,float duration,PartType type )
	{
		Assert.IsTrue( type != PartType.Count );
		if( !loadedParts ) LoadAllParts();

		if( type != PartType.None )
		{
			var curPartObj = GameObject.Instantiate( partPrefabs[( int )type],follow );

			GameObject.Destroy( curPartObj,duration );
		}
	}

	static void LoadPart( PartType type )
	{
		partPrefabs[( int )type] = Resources.Load<GameObject>( "Prefabs/Particles/" + type.ToString() + "Parts" );

		Assert.IsNotNull( partPrefabs[( int )type] );
	}

	static List<GameObject> partPrefabs = new List<GameObject>();

	static bool loadedParts = false;
}
