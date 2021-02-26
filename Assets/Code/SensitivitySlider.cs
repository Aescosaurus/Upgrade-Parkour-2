using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider
	:
	MonoBehaviour
{
	public void Start()
	{
		GetComponent<Slider>().value = PlayerPrefs.GetFloat( "sens",1.0f );
	}

	public void UpdateSens( float val )
	{
		PlayerPrefs.SetFloat( "sens",val );
		PlayerCamCtrl.SetSensitivity( val );
	}
}
