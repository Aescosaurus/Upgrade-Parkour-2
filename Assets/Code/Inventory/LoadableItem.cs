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

		// Assert.IsTrue( invName.Length > 0 );
		// Assert.IsTrue( invDesc.Length > 0 );
	}

	public bool CheckEqual( LoadableItem other )
	{
		// Assert.IsTrue( prefabSrc.Length > 0 );
		// Assert.IsTrue( other.prefabSrc.Length > 0 );
		// print( prefabSrc + " " + other.prefabSrc );
		return( prefabSrc == other.prefabSrc );
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

		invName = other.invName;
		invDesc = other.invDesc;
	}

	// swaps this's data with other
	public void Swap( LoadableItem other )
	{
		var tempPrefSrc = prefabSrc;
		prefabSrc = other.prefabSrc;
		other.prefabSrc = tempPrefSrc;

		var tempPrefab = prefab;
		prefab = other.prefab;
		other.prefab = tempPrefab;
	}

	public void Clear()
	{
		prefabSrc = "";
		prefab = null;
	}

	public GameObject GetPrefab()
	{
		TryLoadPrefab();
		return ( prefab );
	}

	public string GetSrc()
	{
		return( prefabSrc );
	}

	public string GetInvName()
	{
		return( invName );
	}

	public string GetInvDesc()
	{
		return( invDesc );
	}

	[SerializeField] string prefabSrc = "";
	GameObject prefab = null;
	[SerializeField] string invName = "";
	[SerializeField] string invDesc = "";
}
