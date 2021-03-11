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

		pickupText = Instantiate( ResLoader.Load( "Prefabs/HoverText" ) )
			.GetComponentInChildren<TextMesh>();
		pickupText.text = "[E]";

		audSrc = gameObject.AddComponent<AudioSource>();
	}

	protected virtual void Update()
	{
		var diff = player.transform.position - transform.position;
		if( diff.sqrMagnitude < interactDist * interactDist /*&& looking && Vector3.Dot( -diff.normalized,
			( cam.gameObject.transform.forward + Vector3.up * 0.2f ).normalized ) > interactTolerance*/ )
		{
			if( looking )
			{
				pickupText.gameObject.SetActive( true );
				pickupText.transform.position = transform.position + Vector3.up * heightOffset;

				if( SpiffyInput.CheckAxis( "Interact" ) )
				{
					Interact();
					audSrc.PlayOneShot( interactSound );
				}
			}
		}
		else UnInteract();

		looking = false;
	}

	protected abstract void Interact();

	protected virtual void UnInteract()
	{
		pickupText.gameObject.SetActive( false );
	}

	void OnDestroy()
	{
		Destroy( pickupText );
	}

	protected void SetText( string text )
	{
		pickupText.text = text;
	}

	public void Look()
	{
		looking = true;
	}

	[SerializeField] float interactDist = 4.0f;
	[SerializeField] float heightOffset = 1.0f;
	// [SerializeField] float interactTolerance = 0.8f;

	protected GameObject player;
	// Camera cam;

	protected TextMesh pickupText;
	bool looking = false;

	AudioSource audSrc;
	[SerializeField] AudioClip interactSound = null;
}
