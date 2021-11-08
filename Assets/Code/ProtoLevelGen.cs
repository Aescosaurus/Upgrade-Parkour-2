using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoLevelGen
	:
	MonoBehaviour
{
	void Start()
	{
		if( !isClone )
		{
			var floor = transform.GetChild( 0 );
			for( int z = -size.y / 2; z <= size.y / 2; ++z )
			{
				for( int x = -size.x / 2; x <= size.x / 2; ++x )
				{
					if( x == 0 && z == 0 ) continue; // no dupe self

					var clone = Instantiate( gameObject );
					clone.GetComponent<ProtoLevelGen>().isClone = true;
					clone.transform.position = new Vector3(
						x * floor.transform.localScale.x,
						0.0f,
						z * floor.transform.localScale.z );
				}
			}
		}

		for( int i = 1; i < transform.childCount; ++i )
		{
			var curChild = transform.GetChild( i );
			var scale = curChild.transform.localScale;
			var mod = heightMod.Rand();
			scale.y *= mod;

			if( Random.Range( 0.0f,1.0f ) < 0.5f )
			{
				var temp = scale.x;
				scale.x = scale.z;
				scale.z = temp;
			}

			curChild.transform.position += Vector3.down * curChild.transform.position.y;
			curChild.transform.position += Vector3.up * scale.y / 2.0f;
			curChild.transform.localScale = scale;
		}
	}

	[SerializeField] RangeF heightMod = new RangeF( 1.0f,1.0f );

	[SerializeField] Vector3Int size = new Vector3Int( 3,3,3 );

	bool isClone = false;
}
