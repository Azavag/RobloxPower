using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>

namespace J4F{
	public class Singleton<T> : J4F.J4FBehaviour where T : J4F.J4FBehaviour
	{
		private static T _instance;

		
		public static T Instance
		{
			get
			{
				//If _instance hasn't been set yet, we grab it from the scene!
				//This will only happen the first time this reference is used.
				if(_instance == null)
					_instance = GameObject.FindObjectOfType<T>();
				return _instance;
			}
		}

	}
}