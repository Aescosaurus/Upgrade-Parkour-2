using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSelector
	:
	MonoBehaviour
{
	void Start()
	{
		player = GameObject.Find( "Player" );

		pickupText = Instantiate( ResLoader.Load( "Prefabs/HoverText" ) )
			.GetComponentInChildren<TextMesh>();
		pickupText.text = interactMsg;

		origPos = transform.position;
	}

	void Update()
	{
		var diff = player.transform.position - transform.position;
		if( diff.sqrMagnitude < interactDist * interactDist )
		{
			pickupText.gameObject.SetActive( true );
			pickupText.transform.position = transform.position + Vector3.up * heightOffset;

			if( SpiffyInput.CheckAxis( interactAxis ) )
			{
				if( prefAdd.Length > 0 )
				{
					PlayerPrefs.SetInt( prefAdd,1 );
				}

				SceneManager.LoadScene( targetScene );
			}

			transform.Rotate( Vector3.up,spinSpd * Time.deltaTime );
			transform.position = origPos + Vector3.up * upHeight;
		}
		else
		{
			pickupText.gameObject.SetActive( false );
			transform.position = origPos;
		}
	}

	void OnDestroy()
	{
		Destroy( pickupText );
	}

	GameObject player;
	protected TextMesh pickupText;

	float interactDist = 4.5f;
	float heightOffset = 1.0f;
	float spinSpd = 50.0f;
	Vector3 origPos;
	float upHeight = 0.2f;

	[SerializeField] string interactAxis = "";
	[SerializeField] string interactMsg = "";
	[SerializeField] string targetScene = "";
	[SerializeField] string prefAdd = "";
}
