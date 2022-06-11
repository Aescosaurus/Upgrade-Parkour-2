using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHop
	:
	MonoBehaviour
{
	void Start()
	{
		text = GetComponent<Text>();

		scaleReset.Update( scaleReset.GetDuration() );
	}

	void Update()
	{
		scaleReset.Update( Time.deltaTime );

		text.transform.localScale = Vector3.one * Mathf.Lerp( scaleAmount,1.0f,scaleReset.GetPercent() );
	}

	public void Hop()
	{
		text.transform.localScale = Vector3.one * scaleAmount;
		scaleReset.Reset();
	}

	Text text;

	Timer scaleReset = new Timer( 0.3f );
	const float scaleAmount = 1.4f;
}
