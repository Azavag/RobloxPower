using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;


namespace J4F{
	[CustomEditor(typeof(GenerateIcons))]
	public class GenerateIconsEditor : Editor
	{
		private ReorderableList list;

		void OnEnable () {
			list = new ReorderableList(serializedObject, 
			                           serializedObject.FindProperty("prefabsQueue"),
			                           true, false, true, true);
			list.drawHeaderCallback = (Rect rect) => {  
				EditorGUI.LabelField(rect, "Prefabs to iconify");
			};
			list.drawElementCallback =  
			(Rect rect, int index, bool isActive, bool isFocused) => {
				var element = list.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;
				EditorGUI.PropertyField(
					new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
					element, GUIContent.none);

			};
		}
		
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			GenerateIcons myScript = (GenerateIcons)target;

			if(GUILayout.Button("Simple icon generation tool.\nFind support at just4fun.mobi\n(Click to open documentation)", EditorStyles.helpBox))
			{
				Application.OpenURL("http://just4fun.mobi/generate-icons-for-prefabs/");
			}

			EditorGUILayout.Separator ();

			EditorGUILayout.PropertyField (serializedObject.FindProperty("iconSize"), new GUIContent("Target Icon Size"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty("cameraSize"), new GUIContent("Camera size"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty("boundingBoxZone"), new GUIContent("Bounding Box Zone"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty("iconMask"), new GUIContent("Alpha mask"));

			EditorGUILayout.Separator ();EditorGUILayout.Separator ();
			//ListIterator("prefabsQueue");

			EditorGUILayout.Separator ();EditorGUILayout.Separator ();

			EditorGUILayout.PropertyField (serializedObject.FindProperty("prefix"), new GUIContent("Prefix"));
			EditorGUILayout.PropertyField (serializedObject.FindProperty("suffix"), new GUIContent("Suffix"));
			GUILayout.Label("Icons will be saved as "+myScript.prefix+"[prefabname]"+myScript.suffix+".png", EditorStyles.helpBox);
			EditorGUILayout.Separator ();

			EditorGUILayout.PropertyField (serializedObject.FindProperty("iconOutputFolder"), new GUIContent("Output folder"));
			GUILayout.Label("Let this field empty to save icons near prefabs", EditorStyles.helpBox);
			EditorGUILayout.Separator ();
			list.DoLayoutList();
			EditorGUILayout.Separator ();



			// Buttons !
			GUI.color = myScript.useUI ? new Color(0.9f,0.9f,0.9f): new Color(0.6f,0.6f,0.6f);
			if(GUILayout.Button("Use UI" + (myScript.useUI ? " (ON)" : " (OFF)")))
			{
				myScript.SwitchUI();
			}
			GUI.color = Color.white;
			EditorGUILayout.Separator ();
		

			if(GUILayout.Button(myScript.generateItems ? "Save and next !...":"Step by step..."))
			{
				myScript.StepByStep();
			}



			if(myScript.generateItems){
				GUI.color = Color.red;
				if(GUILayout.Button("Running... item "+myScript.currentItem+" (Click to stop generation)"))
				{
					myScript.EndGeneration();
				}
				GUI.color = Color.white;
			}else{
				GUI.color = Color.green;
				if(GUILayout.Button("Start generation"))
				{
					myScript.stepByStep = false;
					myScript.StartGeneration();
				}
				GUI.color = Color.white;
			}

			if(myScript.errorStack != "")
			{
				GUI.color = Color.red;
				GUILayout.Label(myScript.errorStack, EditorStyles.helpBox);
				GUI.color = Color.white;
			}
			
			serializedObject.ApplyModifiedProperties();
		}
	}
}