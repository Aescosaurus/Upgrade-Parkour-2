using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPUI
	:
	MonoBehaviour
{
	void Start()
	{
		xp = PlayerPrefs.GetInt( "xp",0 );

		xpText = GetComponentInChildren<Text>();
	}

	void Update()
	{
		xpText.text = "XP: " + xp.ToString();
	}

	public static void Win()
	{
		++xp;

		PlayerPrefs.SetInt( "xp",xp );
	}

	static int xp = 0;

	Text xpText;
}
