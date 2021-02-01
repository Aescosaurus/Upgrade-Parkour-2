using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class LoadableItem
	:
	MonoBehaviour
{
	void Start()
	{
		TryLoadPrefab();
	}

	public bool CheckEqual( LoadableItem other )
	{
		Assert.IsTrue( prefabSrc.Length > 0 );
		Assert.IsTrue( other.prefabSrc.Length > 0 );
		// print( prefabSrc + " " + other.prefabSrc );
		return( prefabSrc == other.prefabSrc );
	}

	public GameObject GetPrefab()
	{
		TryLoadPrefab();
		return( prefab );
	}

	void TryLoadPrefab()
	{
		if( prefab == null ) prefab = Resources.Load<GameObject>( prefabSrc );
	}

	// copy other's data into this
	public void Copy( LoadableItem other )
	{
		prefabSrc = other.prefabSrc;
		TryLoadPrefab();
	}

	[SerializeField] string prefabSrc = "";
	GameObject prefab = null;
}
