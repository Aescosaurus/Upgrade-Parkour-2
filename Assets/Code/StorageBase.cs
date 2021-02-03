using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using UnityEngine.UI;
using System;

public class StorageBase
	:
	MonoBehaviour
{
	protected virtual void Start()
	{
		// Assert.IsTrue( storagePanelSrc.Length > 0 );
		// 
		// storagePanel = GameObject.Find( storagePanelSrc );

		savePath = Application.persistentDataPath + '/' + gameObject.name + ".txt";

		EnsureFileExists();
		var lines = new List<string>();
		var reader = new StreamReader( savePath );
		while( !reader.EndOfStream ) lines.Add( reader.ReadLine() );
		reader.Close();

		for( int i = 0; i < transform.childCount; ++i )
		{
			slots.Add( transform.GetChild( i ).GetComponent<InventorySlot>() );
		}

		Assert.IsTrue( lines.Count <= slots.Count );
		for( int i = 0; i < lines.Count; ++i )
		{
			var line = lines[i];
			if( line.Length > 0 )
			{
				int stackSize = 1;

				if( char.IsNumber( line[0] ) )
				{
					string counter = "";
					for( int j = 0; j < line.Length; ++j )
					{
						if( line[j] == ' ' )
						{
							try
							{
								stackSize = int.Parse( counter );
							}
							catch( Exception ) {}
							finally
							{
								line = line.Substring( j + 1 );
							}
						}
						else counter += line[j];
					}
				}

				var loadItem = Resources.Load<GameObject>( line ).GetComponent<LoadableItem>();
				for( int j = 0; j < stackSize; ++j )
				{
					slots[i].AddItem( loadItem );
				}
			}
		}

		panelBG = GetComponent<Image>();

		ToggleOpen( false );
	}

	void OnDestroy()
	{
		var writer = new StreamWriter( savePath );

		foreach( var slot in slots )
		{
			string line = "";
			if( slot.GetItem() != null )
			{
				line = slot.GetItem().GetSrc();

				if( slot.CountItems() > 1 ) line = slot.CountItems().ToString() + ' ' + line;
			}
			writer.WriteLine( line );
		}
		writer.Close();
	}

	void EnsureFileExists()
	{
		if( !File.Exists( savePath ) )
		{
			File.Create( savePath );
		}
	}

	protected virtual void ToggleOpen( bool on )
	{
		open = on;
		// gameObject.SetActive( on );
		panelBG.enabled = on;
		foreach( var slot in slots )
		{
			slot.gameObject.SetActive( on );
		}
		Cursor.visible = on;
		Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
	}

	// true if success false if full
	public virtual bool TryAddItem( LoadableItem item )
	{
		foreach( var slot in slots )
		{
			if( slot.TrySetItem( item ) )
			{
				return( true );
			}
		}

		return( false );
	}

	// true if success in stacking false if no stackable items available (including empty slots)
	public virtual bool TryStackItem( LoadableItem item )
	{
		if( CheckExisting( item ) )
		{
			foreach( var slot in slots )
			{
				if( slot.GetItem().CheckEqual( item ) )
				{
					// print( slot.GetItem().GetSrc() + "    " + item.GetSrc() );
					return( slot.TrySetItem( item ) );
				}
			}
		}

		return( false );
		// foreach( var slot in slots )
		// {
		// 	// if( slot.GetItem() == item )
		// 	if( slot.TrySetItem( item ) )
		// 	{
		// 		// slot.TrySetItem( item );
		// 		return( true );
		// 	}
		// }
		// 
		// return( false );
	}

	public bool CheckExisting( LoadableItem checkItem )
	{
		foreach( var slot in slots )
		{
			if( slot.GetItem().CheckEqual( checkItem ) )
			{
				return( true );
			}
		}

		return( false );
	}

	public bool IsOpen()
	{
		return ( open );
	}

	string savePath;

	// [SerializeField] string storagePanelSrc = "";
	// protected GameObject storagePanel;

	Image panelBG;

	protected bool open = false;

	protected List<InventorySlot> slots = new List<InventorySlot>();
}
