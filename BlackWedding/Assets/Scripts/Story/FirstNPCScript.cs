using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class FirstNPCScript : MonoBehaviour
{
	// Set this file to your compiled json asset
	public TextAsset inkAsset;

	// The ink story that we're wrapping
	Story _inkStory;

	void Awake()
	{
		_inkStory = new Story(inkAsset.text); // Construct a new Story object, passing in the JSON string from the TextAsset
	}

    private void Start()
    {
		//Present content

		//while (_inkStory.canContinue)
		//{
		//	Debug.Log(_inkStory.Continue()); // A simpler way to achieve the above is through one call to `_inkStory.ContinueMaximally()
		//}
	}
}
