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
		costLevel = PlayerPrefs.GetInt( gameObject.name + "upgrade",0 );

		nameText = transform.Find( "Text" ).GetComponent<Text>();
		buttonText = transform.Find( "Button" ).GetComponentInChildren<Text>();
		fillImage = transform.Find( "Image" ).GetComponent<Image>();

		RefreshUI();
	}

	public void TryPurchase()
	{
		int xp = XPUI.GetXP();

		if( xp > costTiers[costLevel] )
		{
			print( "yay upgrade" );
			++costLevel;
			PlayerPrefs.SetInt( gameObject.name + "upgrade",costLevel );
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
		buttonText.text = "Upgrade (" + costTiers[costLevel].ToString() + " XP)";
		fillImage.fillAmount = ( float )costLevel / ( float )costTiers.Count;
	}

	Text nameText;
	Text buttonText;
	Image fillImage;

	[SerializeField] List<int> costTiers = new List<int>();

	int costLevel;
}
