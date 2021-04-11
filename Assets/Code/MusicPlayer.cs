using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer
	:
	MonoBehaviour
{
	void Start()
	{
		// todo global volume
		var audSrc = GetComponent<AudioSource>();
		audSrc.clip = music;
		audSrc.loop = true;
		audSrc.Play();
	}

	[SerializeField] AudioClip music = null;
}
