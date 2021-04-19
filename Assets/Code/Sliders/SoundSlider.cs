using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider
	:
	MonoBehaviour
{
	public void Start()
	{
		GetComponent<Slider>().value = PlayerPrefs.GetFloat( "sfx",1.0f );
	}

	public void UpdateSens( float val )
	{
		PlayerPrefs.SetFloat( "sfx",val );
		SoundPlayer.UpdateSoundVol( val );
	}
}
