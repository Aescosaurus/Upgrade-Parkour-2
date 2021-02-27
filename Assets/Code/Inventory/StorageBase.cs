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
	void Awake()
	{
		for( int i = 0; i < transform.childCount; ++i )
		{
			slots.Add( transform.GetChild( i ).GetComponent<InventorySlot>() );
		}

		infoPanel = GameObject.Find( "InfoPanel" ).GetComponent<InfoPanel>();
	}

	protected virtual void Start()
	{
		// Assert.IsTrue( storagePanelSrc.Length > 0 );
		// 
		// storagePanel = GameObject.Find( storagePanelSrc );

		if( writeSaveFile )
		{
			savePath = Application.persistentDataPath + '/' + gameObject.name + ".txt";

			EnsureFileExists();
			var lines = new List<string>();
			var reader = new StreamReader( savePath );
			while( !reader.EndOfStream ) lines.Add( reader.ReadLine() );
			reader.Close();

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
								catch( Exception ) { }
								finally
								{
									line = line.Substring( j + 1 );
								}
							}
							else counter += line[j];
						}
					}

					var loadItem = Resources.Load<GameObject>( line )?.GetComponent<LoadableItem>();
					if( loadItem != null ) slots[i].AddItem( loadItem,stackSize );
					// for( int j = 0; j < stackSize; ++j )
					// {
					// 	slots[i].AddItem( loadItem );
					// }
				}
			}
		}

		panelBG = GetComponent<Image>();
		miscPanel = GameObject.Find( "MiscPanel" );

		if( startClosed ) ToggleOpen( false );
	}

	void OnDestroy()
	{
		if( writeSaveFile )
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
	}

	void EnsureFileExists()
	{
		if( !File.Exists( savePath ) )
		{
			var file = File.Create( savePath );
			file.Close();
		}
	}

	public virtual void ToggleOpen( bool on )
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

		if( !on )
		{
			miscPanel.GetComponentInChildren<VendorUIBase>()?.CloseUI();
			if( miscPanel.transform.childCount > 0 ) Destroy( miscPanel.transform.GetChild( 0 ).gameObject );

			infoPanel.ClosePanel();
		}
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

	// Returns leftover items.
	public virtual int TryStackItem( LoadableItem item,int quantity = 1 )
	{
		// if( CheckExisting( item ) )
		if( quantity > 0 )
		{
			foreach( var slot in slots )
			{
				int space = slot.GetMaxStackSize() - slot.CountItems();
				var stackRemoveSize = Mathf.Min( quantity,space );
				if( slot.CanStack( item,stackRemoveSize ) || slot.CountItems() < 1 )
				{
					if( slot.TrySetItem( item,stackRemoveSize ) ) quantity -= stackRemoveSize;
					// quantity -= space;
					if( quantity < 1 ) break;
				}
			}
		}

		return( quantity );
	}

	public int TryStackExisting( LoadableItem item,int quantity = 1 )
	{
		foreach( var slot in slots )
		{
			int space = slot.GetMaxStackSize() - slot.CountItems();
			var stackRemoveSize = Mathf.Min( quantity,space );
			if( slot.CanStack( item,stackRemoveSize )/* || slot.CountItems() < 1*/ )
			{
				if( slot.TrySetItem( item,stackRemoveSize ) ) quantity -= stackRemoveSize;
				if( quantity < 1 ) break;
			}
		}

		return( quantity );
	}

	// Return true if quantity exists of item and was consumed.
	public virtual bool TryConsumeItem( LoadableItem item,int quantity = 1 )
	{
		bool consumable = false;
		foreach( var slot in slots )
		{
			if( slot.GetItem().CheckEqual( item ) && slot.CountItems() >= quantity )
			{
				consumable = true;
				slot.RemoveItem( quantity );
				break;
			}
		}

		return( consumable );
	}

	public bool CheckExisting( LoadableItem checkItem )
	{
		foreach( var slot in slots )
		{
			if( slot.GetItem().CheckEqual( checkItem ) &&
				slot.GetItem() != checkItem )
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

	public int CountSlots()
	{
		return( slots.Count );
	}

	public List<InventorySlot> GetInvSlots()
	{
		return( slots );
	}

	string savePath;

	// [SerializeField] string storagePanelSrc = "";
	// protected GameObject storagePanel;

	Image panelBG;
	GameObject miscPanel;
	InfoPanel infoPanel;

	protected bool open = false;

	protected List<InventorySlot> slots = new List<InventorySlot>();

	[SerializeField] bool writeSaveFile = true;
	[SerializeField] bool startClosed = true;
}
