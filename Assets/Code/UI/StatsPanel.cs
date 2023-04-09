using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class StatsPanel
	:
	MonoBehaviour
{
	void Start()
	{
		if( self == null ) self = this;

		comboReset.Update( comboReset.GetDuration() );

		coinText = transform.Find( "CoinText" ).GetComponent<Text>();
		coinHop = coinText.GetComponent<TextHop>();
		comboText = transform.Find( "ComboText" ).GetComponent<Text>();
		comboHop = comboText.GetComponent<TextHop>();
		comboMeter = transform.Find( "ComboMeter" ).GetComponent<RectTransform>();

		UpdateStatText();
	}

	void Update()
	{
		if( comboReset.Update( Time.deltaTime / comboMult ) )
		{
			comboMult = 1;
		}

		var comboScale = comboMeter.localScale;
		comboScale.x = 1.0f - comboReset.GetPercent();
		comboMeter.localScale = comboScale;

	}

	void UpdateStatText()
	{
		coinText.text = coinCount.ToString();
		comboText.text = comboMult.ToString() + 'x';
	}

	public static void CollectCoin( int value )
	{
		self.coinCount += value * self.comboMult;
		self.coinHop.Hop();
		self.UpdateStatText();

		++self.comboMult;
		self.comboHop.Hop();
		self.comboReset.Reset();
	}

	public static void AddRemoveCoins( int amount )
	{
		Assert.IsTrue( self.coinCount > amount );

		self.coinCount += amount;
		self.UpdateStatText();
	}

	public static int GetCoinCount()
	{
		return( self.coinCount );
	}

	static StatsPanel self = null;

	int coinCount = 10000;
	int comboMult = 1;

	Text coinText;
	TextHop coinHop;
	Text comboText;
	TextHop comboHop;
	RectTransform comboMeter;

	[SerializeField] Timer comboReset = new Timer( 3.0f );
}
