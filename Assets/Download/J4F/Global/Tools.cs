using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace J4F{
	public class Tools {
		public static void BroadcastMessageToAll(string fun){
			BroadcastMessageToAll (fun, null);
		}
		public static void BroadcastMessageToAll(string fun, System.Object msg) {
			GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
			foreach (GameObject go in gos) {
				if (go && go.transform.parent == null) {
					if(msg == null)
						go.BroadcastMessage(fun, SendMessageOptions.DontRequireReceiver);
					else
						go.BroadcastMessage(fun, msg, SendMessageOptions.DontRequireReceiver);
				}
			}
		}


		public static string FormatTime(float time){
			string result = "";
			result += ((int)time / 60).ToString("00")+":";
			result += ((int)time % 60).ToString("00")+".";
			result += ((int)((time - Mathf.Floor (time)) * 1000)).ToString("000");
			return result;
		}

		public static IEnumerator DeactivateGameObjectPhysic(GameObject obj) {
			yield return new WaitForEndOfFrame();
			obj.SetActive (false);
		}
		
		public static BinaryFormatter bf = new BinaryFormatter ();
		public static void Save (string prefKey, object serializableObject)
		{
			MemoryStream memoryStream = new MemoryStream ();
			bf.Serialize (memoryStream, serializableObject);
			string tmp = System.Convert.ToBase64String (memoryStream.ToArray ());
			PlayerPrefs.SetString ( prefKey, tmp);
		}
		public static T Load<T>(string prefKey)
		{
			if (!PlayerPrefs.HasKey(prefKey))
				return default(T);

			string serializedData = PlayerPrefs.GetString(prefKey);
			MemoryStream dataStream = new MemoryStream(System.Convert.FromBase64String(serializedData));
			
			T deserializedObject = (T)bf.Deserialize(dataStream);
			
			return deserializedObject;
		}

		public static void QualitySwitch(bool value){
			/*if (value)
				Screen.SetResolution (SceneManager.deviceWidth, SceneManager.deviceHeight, true, 60);
			else {
				int myheight = ((SceneManager.deviceHeight*540)/SceneManager.deviceWidth);
				Screen.SetResolution (540, myheight, true, 50);
			}*/
		}

		public static Rect BoundsToScreenRect(Bounds bounds)
		{
			// Get mesh origin and farthest extent (this works best with simple convex meshes)
			Vector3 origin = Camera.main.WorldToViewportPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.min.z));
			Vector3 extent = Camera.main.WorldToViewportPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.max.z));
			
			// Create rect in screen space and return - does not account for camera perspective
			return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
		}

		static Dictionary<string,UnityEngine.Object> preloads;
		public static GameObject InstantiateFromResource(string resourcePath){
			UnityEngine.Object pPrefab;

			if(preloads == null) 
				preloads = new Dictionary<string,UnityEngine.Object>();

			if(!preloads.ContainsKey(resourcePath)){
				preloads.Add(resourcePath, Resources.Load(resourcePath)); // note: not .prefab!
			}
			preloads.TryGetValue(resourcePath, out pPrefab);

			return (GameObject)GameObject.Instantiate (pPrefab);
		}

		public static bool IsUserTouchingUI(){
			if (EventSystem.current.IsPointerOverGameObject ())
							return true;
			foreach (Touch touch in Input.touches)
			{
				int pointerID = touch.fingerId;
				if (EventSystem.current.IsPointerOverGameObject(pointerID))
				{
					// at least on touch is over a canvas UI
					return true;
				}
				
				if (touch.phase == TouchPhase.Ended)
				{
					// here we don't know if the touch was over an canvas UI
					return true;
				}
			}
			return false;
		}

		
	}
}
