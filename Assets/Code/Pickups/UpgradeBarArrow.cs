using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class UpgradeBarArrow
	:
	HoverInteract
{
	protected override void Start()
	{
		base.Start();

		for( int i = 1; i < barCount + 1; ++i )
		{
			var bar = transform.parent.Find( "Bar" + i.ToString() );
			emptyBars[i - 1] = bar.Find( "BarEmpty" ).gameObject;
			fullBars[i - 1] = bar.Find( "BarFull" ).gameObject;
		}

		curLevel = ToolManager.GetEquipLevel( equipType );

		UpdateBarsFilled();
		UpdateInteractText();

		if( isUpgradeArrow ) ToggleShowText( curLevel < barCount );
		else ToggleShowText( curLevel > 1 );
	}

	protected override void OnInteract()
	{
		if( SpiffyInput.CheckAxis( "Interact1" ) )
		{
			bool updatedLevel = false;

			if( isUpgradeArrow )
			{
				if( curLevel < barCount && StatsPanel.GetCoinCount() > upgradeCost[curLevel - 1] )
				{
					StatsPanel.AddRemoveCoins( -upgradeCost[curLevel - 1] );
					++curLevel;
					ToggleShowText( curLevel < barCount );
					updatedLevel = true;
				}
			}
			else
			{
				if( curLevel > 1 )
				{
					--curLevel;
					StatsPanel.AddRemoveCoins( upgradeCost[curLevel - 1] );
					// todo: actually downgrade tool
					ToggleShowText( curLevel > 1 );
					updatedLevel = true;
				}
			}

			if( updatedLevel )
			{
				UpdateBarsFilled();
				UpdateInteractText();

				otherArrow.curLevel = curLevel;
				otherArrow.ToggleShowText( true );
				otherArrow.UpdateInteractText();
				
				ToolManager.SetEquipLevel( equipType,curLevel );

				var tools = FindObjectsOfType<ToolBase>();
				foreach( var tool in tools ) tool.UpdateLevel();
			}
		}
	}

	void UpdateBarsFilled()
	{
		for( int i = 0; i < barCount; ++i )
		{
			emptyBars[i].SetActive( curLevel - 1 >= i );
			fullBars[i].SetActive( curLevel - 1 < i );
		}
	}

	void UpdateInteractText()
	{
		if( isUpgradeArrow && curLevel < barCount )
		{
			interactText.text = interactMsg + " (" + upgradeCost[curLevel - 1].ToString() + " coins)";
		}
	}

	const int barCount = ToolManager.levelCount;
	GameObject[] emptyBars = new GameObject[barCount];
	GameObject[] fullBars = new GameObject[barCount];

	// todo: load from save data & set filled bars
	int curLevel = 1;

	[SerializeField] bool isUpgradeArrow = true;

	[SerializeField] int[] upgradeCost = new int[barCount - 1];

	[SerializeField] UpgradeBarArrow otherArrow = null;

	[SerializeField] PlayerMove2.Equip equipType = PlayerMove2.Equip.None;
}