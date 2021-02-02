using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		return( inv );
	}

	InventoryHandler inv;
}
