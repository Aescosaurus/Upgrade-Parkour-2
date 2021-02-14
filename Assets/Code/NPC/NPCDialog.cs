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

		// if( lines.Count < 1 ) lines.Add( "" );

		if( talkBeforeInteract && lines.Count > 0 )
		{
			++curLine;
			RefreshLine();
		}
	}

	protected override void Interact()
	{
		if( ++curLine > lines.Count - 1 )
		{
			EndDialog();
		}
		else RefreshLine();
	}

	void RefreshLine()
	{
		SetText( lines[curLine] + ( curLine >= lines.Count - 1 ? "" : " ..." ) );
	}

	protected virtual void EndDialog()
	{
		if( lines.Count < 1 ) SetText( "[E]" );
		else if( repeat )
		{
			curLine = talkBeforeInteract ? 0 : -1;
			if( talkBeforeInteract )
			{
				curLine = 0;
				RefreshLine();
			}
			else
			{
				curLine = -1;
				SetText( "[E]" );
			}
		}
		else SetText( "" );
	}

	[SerializeField] bool talkBeforeInteract = false;
	[SerializeField] bool repeat = true;
	[SerializeField] protected List<string> lines = new List<string>();
	protected int curLine = -1;
}
