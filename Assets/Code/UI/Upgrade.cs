using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

		nameText = transform.Find( "Text" ).GetComponent<Text>();
		upgradeButton = transform.Find( "Button" ).GetComponent<Button>();
		buttonText = upgradeButton.GetComponentInChildren<Text>();
		fillImage = transform.Find( "Image" ).GetComponent<Image>();

		RefreshUI();
	}

	public void TryPurchase()
	{
		int xp = XPUI.GetXP();

		if( xp >= costTiers[costLevel] )
		{
			// print( "yay upgrade" );
			XPUI.AddXP( -costTiers[costLevel] );
			++costLevel;
			SetLevel();
			RefreshUI();
		}
		else
		{
			// print( "no go" );
		}
	}

	void RefreshUI()
	{
		nameText.text = gameObject.name;

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

	Text nameText;
	Button upgradeButton;
	Text buttonText;
	Image fillImage;

	/*[SerializeField]*/ public static bool reset = false;
	[SerializeField] List<int> costTiers = new List<int>();

	int costLevel = 0;

	bool started = false;
}
