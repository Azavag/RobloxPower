using UnityEngine;
using System.Collections;
using UnityEditor;

namespace J4F{
	[CustomEditor(typeof(QueueProvider),true)]
	public class QueueProviderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			QueueProvider myScript = (QueueProvider)target;


			if(!myScript.enabled){
				GUI.color = Color.red;
				GUILayout.Label("This provider WILL NOT BE used on export. Enable this script to use it.", EditorStyles.helpBox);
				GUI.color = Color.white;
			}else{
				GUI.color = Color.green;
				GUILayout.Label("This provider WILL BE used on export. Disable this script to ignore it.", EditorStyles.helpBox);
				GUI.color = Color.white;
			}

			DrawDefaultInspector ();
		}
	}
}
