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
		LoadXP();

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

		// AddXP( 1 );
		AddXP( 5 );

		upgradePanel.LateOpen();
	}

	public static void AddXP( int amount )
	{
		xp += amount;

		PlayerPrefs.SetInt( "xp",xp );
	}

	static void LoadXP()
	{
		xp = PlayerPrefs.GetInt( "xp",0 );
	}

	public static int GetXP()
	{
		if( xp < 0 ) LoadXP();
		return( xp );
	}

	static int xp = -1;

	Text xpText;

	static UpgradePanel upgradePanel;
}
