using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernGenerator
	:
	MonoBehaviour
{
	void Start()
	{
		tunnelPrefab = ResLoader.Load( "Prefabs/Cavern/CavernTunnel" );
		roomPrefab = ResLoader.Load( "Prefabs/Cavern/Room" );
		endPrefab = ResLoader.Load( "Prefabs/Cavern/CavernEnd" );

		var doors = transform.Find( "Doors" );

		var tunnels = new List<int>();
		for( int i = 0; i < doors.childCount; ++i ) tunnels.Add( Random.Range( 0,4 ) );

		for( int i = 0; i < doors.childCount; ++i )
		{
			if( tunnels[i] == 1 || tunnels[i] == 3 )
			{
				GenerateTunnel( doors.GetChild( i ).Find( "TopLevel" ),CanSpawnBranches( tunnels,i,true ) );
			}

			if( tunnels[i] == 2 || tunnels[i] == 3 )
			{
				GenerateTunnel( doors.GetChild( i ).Find( "BotLevel" ),CanSpawnBranches( tunnels,i,false ) );
			}
		}
	}

	void GenerateTunnel( Transform level,bool allowBranches )
	{
		var tunnel = Instantiate( tunnelPrefab,transform );
		tunnel.transform.position = level.position;
		tunnel.transform.rotation = level.transform.rotation;
		tunnel.transform.Rotate( Vector3.up,180.0f );

		var room = Instantiate( roomPrefab,transform );
		room.transform.position = tunnel.transform.Find( "End" ).position;
		room.transform.rotation = tunnel.transform.rotation;
		var roomScr = room.GetComponent<CavernRoom>();
		var exitDoor = roomScr.Generate( allowBranches );
		StartCoroutine( PopulateRoom( tunnel.GetComponent<CavernRoom>() ) );
		StartCoroutine( PopulateRoom( roomScr ) );

		if( exitDoor != null )
		{
			var t2 = Instantiate( tunnelPrefab,transform );
			t2.transform.position = exitDoor.transform.position;
			t2.transform.rotation = exitDoor.transform.rotation;
			StartCoroutine( PopulateRoom( t2.GetComponent<CavernRoom>() ) );
			// t2.transform.Rotate( Vector3.up,180.0f );
			var endPiece = Instantiate( endPrefab,transform );
			endPiece.transform.position = t2.transform.Find( "End" ).position;
			endPiece.transform.rotation = t2.transform.rotation;
			Destroy( exitDoor.gameObject );
		}

		Destroy( level.gameObject );
	}

	bool CanSpawnBranches( List<int> tunnels,int i,bool top )
	{
		var l = i - 1;
		var r = i + 1;
		if( l < 0 ) l = tunnels.Count - 1;
		if( r >= tunnels.Count ) r = 0;
		
		var lval = tunnels[l];
		var rval = tunnels[r];

		if( top ) return( ( lval == 0 || lval == 2 ) && ( rval == 0 || rval == 2 ) );
		else return( ( lval == 0 || lval == 1 ) && ( rval == 0 || rval == 1 ) );
	}

	IEnumerator PopulateRoom( CavernRoom roomScr )
	{
		yield return( new WaitForEndOfFrame() );
		roomScr.PopulateRoom( decoPrefabs,decoSpawnChance,enemyPrefabs );
	}

	GameObject tunnelPrefab;
	GameObject roomPrefab;
	GameObject endPrefab;

	[SerializeField] float decoSpawnChance = 0.2f;
	[SerializeField] List<GameObject> decoPrefabs = new List<GameObject>();
	[SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
}
