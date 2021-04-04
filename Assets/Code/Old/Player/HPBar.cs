using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar
	:
	MonoBehaviour
{
	void Start()
	{
		barImg = GameObject.Find( "Canvas" ).transform.Find( "BarPanel" ).Find( "HPBar" ).GetComponent<Image>();
		dmgable = GetComponent<Damageable>();
	}

	void Update()
	{
		barImg.fillAmount = dmgable.GetHPPercent();
	}

	Image barImg;
	Damageable dmgable;
}
