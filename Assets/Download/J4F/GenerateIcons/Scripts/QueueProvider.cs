using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace J4F{
	public class QueueProvider : MonoBehaviour
	{
		void Start(){

		}

		/// <summary>
		/// Return GameObjects.
		/// The GenerationIcons script will add this list to the generation queue 
		/// </summary>
		/// <returns>A prefab list</returns>
		virtual public List<GameObject> GetPrefabs(){
			// To add your own automation, just extend this class and override this method as in the AutomateQueueSample.
			// Build your own gameobjects 
			return new List<GameObject>();
		}
	}
}

