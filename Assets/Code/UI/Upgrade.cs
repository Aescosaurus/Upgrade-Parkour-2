using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class Upgrade
	:
	MonoBehaviour
{
	public void Start()
	{
		if( started ) return;
		started = true;

		if( !reset ) costLevel = PlayerPrefs.GetInt( gameObject.name + " upgrade",0 );
		SetLevel();

		nameText = transform.Find( "UpgradeName" ).GetComponent<Text>();
		xpBonusText = transform.Find( "XPBonusText" ).GetComponent<Text>();
		upgradeButton = transform.Find( "UpgradeButton" ).GetComponent<Button>();
		helpButtonText = transform.Find( "HelpButton" ).GetComponentInChildren<Text>();
		buttonText = upgradeButton.GetComponentInChildren<Text>();
		fillImage = transform.Find( "Image" ).GetComponent<Image>();
		infoText = transform.Find( "InfoText" ).GetComponent<Text>();

		infoText.enabled = false;

		Assert.IsTrue( descs.Count == costTiers.Count + 1 );

		RefreshUI();
	}

	public void TryPurchase()
	{
		int xp = XPUI.GetXP();

		if( xp >= costTiers[costLevel] )
		{
			// print( "yay upgrade" );
			XPUI.AddXP( -costTiers[costLevel] );
			XPUI.AddXPBonus( xpBonusPercent );
			
			++costLevel;
			SetLevel();
			RefreshUI();
			var otherUpgrades = FindObjectsOfType<Upgrade>();
			foreach( var other in otherUpgrades ) other.RefreshUI();
		}
		else
		{
			// print( "no go" );
		}
	}

	void RefreshUI()
	{
		nameText.text = gameObject.name;
		xpBonusText.text = "xp bonus: " + xpBonusPercent.ToString() + "%";

		var fill = ( float )costLevel / ( float )costTiers.Count;
		if( fill < 0.0f ) fill = 0.0f;
		if( fill > 1.0f ) fill = 1.0f;
		fillImage.fillAmount = fill;

		if( costLevel >= costTiers.Count )
		{
			upgradeButton.interactable = false;
			buttonText.text = "";
		}
		else
		{
			upgradeButton.interactable = XPUI.GetXP() >= costTiers[costLevel];
			buttonText.text = "Upgrade (" + costTiers[costLevel].ToString() + " XP)";
		}
	}

	void SetLevel()
	{
		PlayerPrefs.SetInt( gameObject.name + " upgrade",costLevel );
	}

	void OnEnable()
	{
		if( started ) RefreshUI();
	}

	public void HelpSwap()
	{
		// todo spiffy flip animation
		helpOpen = !helpOpen;

		infoText.text = descs[costLevel];

		infoText.enabled = helpOpen;
		nameText.enabled = !helpOpen;
		xpBonusText.enabled = !helpOpen;
		upgradeButton.gameObject.SetActive( !helpOpen );
		fillImage.enabled = !helpOpen;

		helpButtonText.text = ( helpOpen ? "X" : "?" );
	}

	Text nameText;
	Text xpBonusText;
	Button upgradeButton;
	Text helpButtonText;
	Text buttonText;
	Image fillImage;
	Text infoText;

	/*[SerializeField]*/ public static bool reset = false;
	// x / 100
	[SerializeField] int xpBonusPercent = 0;
	[SerializeField] List<int> costTiers = new List<int>();
	[SerializeField] List<string> descs = new List<string>();

	int costLevel = 0;

	bool started = false;

	bool helpOpen = false;
}
