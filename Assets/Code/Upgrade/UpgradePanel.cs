using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradePanel
	:
	MonoBehaviour
{
	void Start()
	{
		img = GetComponent<Image>();
		origOpacity = img.color.a;

		for( int i = 0; i < transform.childCount; ++i )
		{
			children.Add( transform.GetChild( i ).gameObject );
			children[i].GetComponent<Upgrade>()?.Start();
		}

		tutPanel = FindObjectOfType<TutorialPanel>();
		pauseMenu = FindObjectOfType<PauseMenu>();

		ToggleOpen( false );

		FindObjectOfType<ForestGenerator>().Generate();
	}

	void Update()
	{
		// if( SpiffyInput.CheckAxis( "Inventory",true ) ) ToggleOpen( !open );
	}

	public void ToggleOpen( bool open )
	{
		this.open = open;

		// var c = img.color;
		// c.a = open ? origOpacity : 0.0f;
		// img.color = c;
		img.enabled = open;

		foreach( var child in children )
		{
			child.SetActive( open );
		}

		PauseMenu.SetOpen( open );

		if( open )
		{
			Destroy( tutPanel.gameObject );
			Destroy( pauseMenu.gameObject );
		}

		// Cursor.visible = open;
		// Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
	}

	public void RegenDungeon()
	{
		Upgrade.UnReset();

		SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
	}

	public void LateOpen()
	{
		StartCoroutine( ToggleOpenLate( 1.0f ) );
	}

	IEnumerator ToggleOpenLate( float t )
	{
		yield return( new WaitForSeconds( t ) );

		ToggleOpen( true );
	}

	Image img;
	float origOpacity;
	List<GameObject> children = new List<GameObject>();

	TutorialPanel tutPanel;
	PauseMenu pauseMenu;

	bool open = false;
}