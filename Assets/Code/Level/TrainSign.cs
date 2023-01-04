using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSign
	:
	MonoBehaviour
{
	void Start()
	{
		modelOn = transform.Find( "TrainSign3Lit" ).gameObject;
		modelOff = transform.Find( "TrainSign3" ).gameObject;

		startPos = transform.position;
		StartCoroutine( Flicker() );
	}

	IEnumerator Flicker()
	{
		yield return( new WaitForSeconds( flickerTimer ) );

		on = !on;
		modelOn.SetActive( on );
		modelOff.SetActive( !on );

		StartCoroutine( Flicker() );
	}

	[SerializeField] float flickerTimer = 1.0f;

	bool on = false;
	Vector3 startPos;

	GameObject modelOn;
	GameObject modelOff;
}