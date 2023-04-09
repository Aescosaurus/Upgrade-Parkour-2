using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverInteract
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
		cam = Camera.main;

		rayMask = LayerMask.GetMask( MaskString );
		
		interactText = Instantiate( ResLoader.Load( "Prefabs/HoverText" ) )
			.GetComponentInChildren<TextMesh>();
		interactText.text = interactMsg;
		interactText.transform.position = transform.position + Vector3.up * heightOffset;
		interactText.gameObject.SetActive( false );
	}

	protected virtual void Update()
	{
		bool oldActive = hoverActive;
		hoverActive = false;

		var dist = player.transform.position - transform.position;
		if( dist.sqrMagnitude < interactDist * interactDist )
		{
			var ray = new Ray( cam.transform.position,cam.transform.forward );
			RaycastHit hit;

			if( Physics.Raycast( ray,out hit,interactDist * 2,rayMask ) )
			{
				if( hit.transform.gameObject == gameObject )
				{
					OnInteract();
					hoverActive = true;
				}
			}
		}

		if( hoverActive != oldActive )
		{
			OnInteractToggle( hoverActive );
			interactText.gameObject.SetActive( showText && hoverActive );
		}
	}

	protected void ToggleShowText( bool show )
	{
		showText = show;

		if( !show ) interactText.gameObject.SetActive( false );
	}

	protected virtual void OnInteract() {}
    protected virtual void OnInteractToggle( bool on ) {}

    protected GameObject player;
	Camera cam;
	protected TextMesh interactText;
	
	[Tooltip( "Name of the layer your object is on" )]
	[SerializeField] string MaskString = "ToolPickup";
	LayerMask rayMask;

	[SerializeField] float interactDist = 3.0f;

	bool hoverActive = false;
	
	[SerializeField] protected string interactMsg = "[E]/[Q] Pickup";
	[SerializeField] float heightOffset = 1.0f;

	bool showText = true;
}