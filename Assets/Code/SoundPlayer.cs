using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer
	:
	MonoBehaviour
{
	void Start()
	{
		audSrc = GetComponent<AudioSource>();
		UpdateSoundVol( PlayerPrefs.GetFloat( "sfx",1.0f ) );
	}

	public static void UpdateSoundVol( float newVol )
	{
		audSrc.volume = newVol;
	}

	static AudioSource audSrc = null;
}
