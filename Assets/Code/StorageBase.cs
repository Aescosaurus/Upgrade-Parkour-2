using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using UnityEngine.UI;

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
				slots[i].AddItem( Resources.Load<GameObject>( line ).GetComponent<LoadableItem>() );
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
				// print( line );
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
