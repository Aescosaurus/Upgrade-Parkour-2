using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog
	:
	InteractiveBase
{
	protected override void Start()
	{
		base.Start();

		if( lines.Count < 1 ) lines.Add( "" );

		if( talkBeforeInteract )
		{
			++curLine;
			RefreshLine();
		}
	}

	protected override void Interact()
	{
		if( ++curLine > lines.Count - 1 )
		{
			SetText( "" );
			if( repeat ) curLine = -1;
		}
		else RefreshLine();
	}

	void RefreshLine()
	{
		SetText( lines[curLine] );
	}

	[SerializeField] bool talkBeforeInteract = false;
	[SerializeField] bool repeat = true;
	[SerializeField] List<string> lines = new List<string>();
	int curLine = -1;
}
