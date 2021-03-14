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
		upgradePanel = FindObjectOfType<UpgradePanel>();
	}

	void Update()
	{
		xpText.text = "XP: " + xp.ToString();
	}

	public static void Win()
	{
		// ++xp;
		// 
		// PlayerPrefs.SetInt( "xp",xp );
		AddXP( 1 );

		upgradePanel.LateOpen();
	}

	public static void AddXP( int amount )
	{
		xp += amount;

		PlayerPrefs.SetInt( "xp",xp );
	}

	public static int GetXP()
	{
		return( xp );
	}

	static int xp = 0;

	Text xpText;

	static UpgradePanel upgradePanel;
}
