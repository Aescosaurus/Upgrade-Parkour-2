using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerInventory
	:
	MonoBehaviour
{
	void Start()
	{
		inv = FindObjectOfType<InventoryHandler>();
	}

	public InventoryHandler GetInv()
	{
		Assert.IsNotNull( inv );
		if( inv == null ) inv = FindObjectOfType<InventoryHandler>();
		return( inv );
	}

	InventoryHandler inv = null;
}
