#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace J4F{
	public class EditorTools {
		
		public static void SaveIconToFolder(GameObject prefab){
			if (prefab != null) {
				Texture2D texture = AssetPreview.GetAssetPreview (prefab);
				
				if(texture != null){
					EditorTools.SaveTextToPath(EditorTools.GetResourcesPath(prefab), texture);
				}
			}
		}


		public static string GetResourcesPath(UnityEngine.Object prefab){
			string path = AssetDatabase.GetAssetPath (prefab).Substring(7);
			path = Application.dataPath + "/" + path.Remove(path.LastIndexOf("/")) + "/";
			return path;
		}
		
		public static void SaveTextToPath(string path, Texture2D text){
			#if !UNITY_WEBPLAYER
			File.WriteAllBytes(path, text.EncodeToPNG());
			#endif
		}
		
	}
	
}
#endif