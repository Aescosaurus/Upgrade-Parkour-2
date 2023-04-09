using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoverInteract
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		player = GameObject.FindGameObjectWithTag( "Player" );
		cam = Camera.main;

		rayMask = LayerMask.GetMask( "ToolPickup" );
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

		if( hoverActive != oldActive ) OnInteractToggle( hoverActive );
	}

	protected abstract void OnInteract();
    protected abstract void OnInteractToggle( bool on );

    protected GameObject player;
	Camera cam;
	
	[SerializeField] string MaskString = "ToolPickup";
	LayerMask rayMask;

	[SerializeField] float interactDist = 3.0f;

	bool hoverActive = false;
}