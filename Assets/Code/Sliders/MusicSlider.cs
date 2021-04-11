using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider
	:
	MonoBehaviour
{
	public void Start()
	{
		GetComponent<Slider>().value = PlayerPrefs.GetFloat( "music",1.0f );
	}

	public void UpdateSens( float val )
	{
		PlayerPrefs.SetFloat( "music",val );
		MusicPlayer.UpdateMusicVol( val );
	}
}
