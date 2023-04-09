using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager
{
	public static void EquipItem( PlayerMove2.Equip equip,int hand )
	{
		// todo: save these & load in PlayerMove2 Start
		PlayerPrefs.SetInt( "wep" + hand.ToString(),( int )equip );
	}

	public static void SetEquipLevel( PlayerMove2.Equip equip,int level )
	{
		PlayerPrefs.SetInt( equip.ToString() + "_lvl",level );
	}

	public static PlayerMove2.Equip GetHandEquip( int hand )
	{
		string handKey = "wep" + hand.ToString();
		if( PlayerPrefs.HasKey( handKey ) ) return( ( PlayerMove2.Equip )PlayerPrefs.GetInt( handKey ) );
		else return( PlayerMove2.Equip.None );
	}

	public static int GetEquipLevel( PlayerMove2.Equip type )
	{
		string equipKey = type.ToString() + "_lvl";
		if( PlayerPrefs.HasKey( equipKey ) ) return( PlayerPrefs.GetInt( equipKey ) );
		else return( 1 );
	}
}