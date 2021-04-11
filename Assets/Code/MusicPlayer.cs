using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer
	:
	MonoBehaviour
{
	void Start()
	{
		audSrc = GetComponent<AudioSource>();
		UpdateMusicVol( PlayerPrefs.GetFloat( "music",1.0f ) );
		audSrc.clip = music;
		audSrc.loop = true;
		audSrc.Play();
	}

	public static void UpdateMusicVol( float newVol )
	{
		audSrc.volume = newVol;
	}

	static AudioSource audSrc = null;

	[SerializeField] AudioClip music = null;
}
