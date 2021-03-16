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

		xpText = transform.Find( "XPText" ).GetComponent<Text>();
		xpBonusText = transform.Find( "XPBonusText" ).GetComponent<Text>();
		upgradePanel = FindObjectOfType<UpgradePanel>();
	}

	void Update()
	{
		xpText.text = "XP: " + xp.ToString();
		xpBonusText.text = "xp bonus: " + xpBonus.ToString() + "%";
	}

	public static void Win()
	{
		// ++xp;
		// 
		// PlayerPrefs.SetInt( "xp",xp );

		// AddXP( 1 );
		AddXP( 10 );

		upgradePanel.LateOpen();
	}

	public static void AddXP( int amount )
	{
		xp += ( int )Mathf.Ceil( ( ( float )amount ) * ( 1.0f + ( float )xpBonus / 100.0f ) );

		PlayerPrefs.SetInt( "xp",xp );
	}

	public static void AddXPBonus( int amount )
	{
		xpBonus += amount;

		PlayerPrefs.SetInt( "xpbonus",xpBonus );
	}

	public static void ResetAll()
	{
		xp = 0;
		xpBonus = 0;
	}

	static void LoadXP()
	{
		xp = PlayerPrefs.GetInt( "xp",0 );
		xpBonus = PlayerPrefs.GetInt( "xpbonus",0 );
	}

	public static int GetXP()
	{
		if( xp < 0 ) LoadXP();
		return( xp );
	}

	static int xp = -1;
	static int xpBonus = 0;

	Text xpText;
	Text xpBonusText;

	static UpgradePanel upgradePanel;
}
