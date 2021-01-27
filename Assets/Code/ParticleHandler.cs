using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ParticleHandler
	:
	MonoBehaviour
{
	public enum ParticleType
	{
		None,
		Ouch,
		Smoke,
		Count
	}

	void Awake()
	{
		for( int i = 0; i < ( int )ParticleType.Count; ++i ) particlePrefabs.Add( null );

		// particlePrefabs[( int )ParticleType.Ouch] = Resources.Load<GameObject>( "Prefabs/Particle/OuchPart" );
		LoadParticle( ParticleType.Ouch,"OuchPart" );
		LoadParticle( ParticleType.Smoke,"SmokePart" );
	}

	public void SpawnParticles( Vector3 loc,int amount,ParticleType type )
	{
		Assert.IsTrue( type != ParticleType.Count );

		if( type != ParticleType.None )
		{
			var curPartObj = Instantiate( particlePrefabs[( int )type],loc,Quaternion.identity );
			// curPartObj.transform.position = loc;

			var partSys = curPartObj.GetComponent<ParticleSystem>();
			partSys.Emit( amount );
			Destroy( curPartObj,partSys.main.duration );
		}
	}

	void LoadParticle( ParticleType type,string path )
	{
		particlePrefabs[( int )type] = Resources.Load<GameObject>( "Prefabs/Particle/" + path );
	}

	List<GameObject> particlePrefabs = new List<GameObject>();
}
