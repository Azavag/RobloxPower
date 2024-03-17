using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Make your own extended class and add them to the "Generation Cam" object
/// In this sample, just a public list
/// If this script is enabled, the gameobjects in this list will be added to queue
/// </summary>

// 1. Extend the J4F.QueueProvider class
public class AdditionnalQueueProvider : J4F.QueueProvider
{
	public List<GameObject> addPrefabList;

	// 2. Override this function to provide a Gameobject list
	public override List<GameObject> GetPrefabs ()
	{
		return addPrefabList;
	}
}

