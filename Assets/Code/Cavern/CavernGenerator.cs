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

		bossSpawnLoc = transform.Find( "BossSpawnLoc" );
		bossPrefab = ResLoader.Load( "Prefabs/Enemy/Boss/CrystalBoss" );

		var doors = transform.Find( "Doors" );

		var tunnels = new List<int>();
		if( nPossibleTunnels > doors.childCount ) nPossibleTunnels = doors.childCount;
		for( int i = 0; i < nPossibleTunnels; ++i ) tunnels.Add( Random.Range( 1,4 ) );
		for( int i = 0; i < doors.childCount - nPossibleTunnels; ++i ) tunnels.Add( 0 );

		for( int i = 0; i < tunnels.Count; ++i )
		{
			var randSpot = Random.Range( 0,tunnels.Count );
			var temp = tunnels[i];
			tunnels[i] = tunnels[randSpot];
			tunnels[randSpot] = temp;
		}

		foreach( var tunnel in tunnels )
		{
			if( tunnel >= 3 ) nTunnels += 2;
			else if( tunnel >= 1 ) nTunnels += 1;
		}

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
			StartCoroutine( PopulateRoom( t2.GetComponent<CavernRoom>(),true ) );
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

	IEnumerator PopulateRoom( CavernRoom roomScr,bool spawnChest = false )
	{
		yield return( new WaitForEndOfFrame() );
		roomScr.PopulateRoom( decoPrefabs,decoSpawnChance,enemyPrefabs,spawnChest );
	}

	public static int CollectDeco( int amount )
	{
		nDeco += amount;

		if( nDeco == 0 )
		{
			var boss = Instantiate( bossPrefab );
			boss.transform.position = bossSpawnLoc.position;
		}

		return( nDeco );
	}

	GameObject tunnelPrefab;
	GameObject roomPrefab;
	GameObject endPrefab;

	[SerializeField] float decoSpawnChance = 0.2f;
	[SerializeField] List<GameObject> decoPrefabs = new List<GameObject>();
	[SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

	[SerializeField] int nPossibleTunnels = 3;
	int nTunnels;

	static int nDeco = 0;
	static Transform bossSpawnLoc;
	static GameObject bossPrefab;
}
