using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetup
	:
	MonoBehaviour
{
	void Start()
	{
		GetComponent<Canvas>().worldCamera = Camera.main.transform.GetChild( 0 ).GetComponent<Camera>();
	}
}
