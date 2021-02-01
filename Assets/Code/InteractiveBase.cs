using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveBase
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		player = GameObject.Find( "Player" );
		// cam = Camera.main;

		pickupText = Instantiate( Resources.Load<GameObject>( "Prefabs/HoverText" ) );
		pickupText.GetComponentInChildren<TextMesh>().text = "[E]";
	}

	void Update()
	{
		var diff = player.transform.position - transform.position;
		if( diff.sqrMagnitude < interactDist * interactDist )
		{
			pickupText.SetActive( true );
			pickupText.transform.position = transform.position + Vector3.up * heightOffset;

			if( SpiffyInput.CheckAxis( "Interact" ) ) Interact();
		}
		else pickupText.SetActive( false );
	}

	protected abstract void Interact();

	void OnDestroy()
	{
		Destroy( pickupText );
	}

	[SerializeField] float interactDist = 4.0f;
	[SerializeField] float heightOffset = 1.0f;

	protected GameObject player;
	// Camera cam;

	GameObject pickupText;
}
