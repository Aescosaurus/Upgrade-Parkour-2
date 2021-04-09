using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton
	:
	MonoBehaviour
{
	void Start()
	{
		audSrc = gameObject.AddComponent<AudioSource>();
	}

	public void Hover()
	{
		// audSrc.PlayOneShot( hoverSound );
	}

	public void Click()
	{
		// var audLeftover = Instantiate( ResLoader.Load( "Prefabs/AudioLeftover" ) );
		// audLeftover.AddComponent<AudioSource>().PlayOneShot( clickSound );
		// Destroy( audLeftover,clickSound.length );
	}

	AudioSource audSrc;

	// [SerializeField] AudioClip hoverSound = null;
	// [SerializeField] AudioClip clickSound = null;
}
