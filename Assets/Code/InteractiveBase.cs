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
		cam = Camera.main;

		pickupText = Instantiate( Resources.Load<GameObject>( "Prefabs/HoverText" ) )
			.GetComponentInChildren<TextMesh>();
		pickupText.text = "[E]";
	}

	protected virtual void Update()
	{
		var diff = player.transform.position - transform.position;
		if( diff.sqrMagnitude < interactDist * interactDist && Vector3.Dot( -diff.normalized,
			( cam.gameObject.transform.forward + Vector3.up * 0.2f ).normalized ) > interactTolerance )
		{
			pickupText.gameObject.SetActive( true );
			pickupText.transform.position = transform.position + Vector3.up * heightOffset;

			if( SpiffyInput.CheckAxis( "Interact" ) ) Interact();
		}
		else pickupText.gameObject.SetActive( false );
	}

	protected abstract void Interact();

	void OnDestroy()
	{
		Destroy( pickupText );
	}

	protected void SetText( string text )
	{
		pickupText.text = text;
	}

	[SerializeField] float interactDist = 4.0f;
	[SerializeField] float heightOffset = 1.0f;
	[SerializeField] float interactTolerance = 0.8f;

	protected GameObject player;
	Camera cam;

	TextMesh pickupText;
}
