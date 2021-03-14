using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade
	:
	MonoBehaviour
{
	void Start()
	{
		if( !reset ) costLevel = PlayerPrefs.GetInt( gameObject.name + " upgrade",0 );

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
			print( "yay upgrade" );
			XPUI.AddXP( -costTiers[costLevel] );
			++costLevel;
			PlayerPrefs.SetInt( gameObject.name + " upgrade",costLevel );
			RefreshUI();
		}
		else
		{
			print( "no go" );
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

	Text nameText;
	Button upgradeButton;
	Text buttonText;
	Image fillImage;

	[SerializeField] bool reset = false;
	[SerializeField] List<int> costTiers = new List<int>();

	int costLevel;
}
