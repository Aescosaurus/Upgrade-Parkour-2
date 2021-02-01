﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal
	:
	InteractiveBase
{
	protected override void Interact()
	{
		// todo check player stats before allowing entry
		SceneManager.LoadScene( worldTarget );
	}

	[SerializeField] string worldTarget = "";
}