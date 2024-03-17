#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace J4F{
	[ExecuteInEditMode]
	public class GenerateIcons : MonoBehaviour
	{
		public GameObject boundingBoxZone;
		public int iconSize = 256;
		public Texture iconMask;
		public float cameraSize = 5f;
		public string prefix;
		public string suffix = "_icon";
		public List<GameObject> prefabsQueue;
		public List<GameObject> completeQueue;
		public string iconOutputFolder = "";
		public bool useUI = true;
		public bool generateItems = false;
		public bool stepByStep = false;
		public bool doStep = false;
		public string errorStack = "";

		Camera generationCam;
		GameObject currentGameObject;
		public int currentItem = 0;
		int okItem = 0;

		/// <summary>
		/// Generates the icons.
		/// Will display a button in play mode
		/// </summary>
		public void StartGeneration(){

			errorStack = "";
			if(currentGameObject != null) DestroyImmediate(currentGameObject.gameObject);
			// Prepare the complete queue :
			completeQueue = new List<GameObject>();
			// add manual prefabs queue :
			completeQueue.AddRange(prefabsQueue);
			// Add optionnal automated queues :
			QueueProvider[] queues = GetComponents<QueueProvider>();
			if(queues != null){
				foreach(QueueProvider queue in queues){
					if(queue.GetPrefabs() != null && queue.enabled)
						completeQueue.AddRange(queue.GetPrefabs());
				}
			}


			if(completeQueue != null && completeQueue.Count > 0){
				Debug.Log("GenerateIcons - Generation started : " + completeQueue.Count + " elements to do...");
				generateItems = true;
				okItem = 0;
				currentItem = 0;
				if(boundingBoxZone != null) boundingBoxZone.SetActive(false);
				EditorApplication.update += PrepareIcon;
			}else{
				Error("GenerateIcons - Generation can't start : no prefab in the list !");
			}
		}

		public void StepByStep(){
			if(!generateItems){
				stepByStep = true;
				doStep = true;
				if(!generateItems)
					StartGeneration();
			} else {
				SaveIt();
				EditorUtility.SetDirty (this);
			}
		}

		/// <summary>
		/// Ends the icons generation.
		/// </summary>
		public void EndGeneration(){
			if(currentGameObject != null) DestroyImmediate(currentGameObject.gameObject);
			Debug.Log("GenerateIcons - End. " + okItem + " icons created");
			generateItems = false;
			if(boundingBoxZone != null) boundingBoxZone.SetActive(true);
			EditorApplication.update -= PrepareIcon;
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// All preliminary stuff to put the prefab properly on screen.
		/// </summary>
		void PrepareIcon ()
		{
			if(generateItems && completeQueue != null && (!stepByStep || doStep)){
				doStep = false;
				if(currentItem<completeQueue.Count){
					try{


						//instantiate GO and deactivate Animator
						currentGameObject = ((GameObject) MonoBehaviour.Instantiate (completeQueue[currentItem]));
						currentGameObject.transform.position = Vector3.zero;
						currentGameObject.transform.localScale = Vector3.one;
						if(currentGameObject.GetComponent<Animator>() != null){
							currentGameObject.GetComponent<Animator>().enabled = false;
						}

						// estimate bounds
						Bounds bounds = GameObjectExtensions.GetGlobalBounds(currentGameObject);
						
						// center the object despite his pivot point
						currentGameObject.transform.position = - bounds.center; 

						// find the biggest edge
						float baseSize = Mathf.Max(bounds.size.x,Mathf.Max(bounds.size.y, bounds.size.z));

						float scale = 5f/baseSize;
						currentGameObject.transform.localScale = new Vector3(scale,scale,scale);

						currentGameObject.transform.position = Vector3.zero;
						// finally recenter the object after scale
						currentGameObject.transform.position = - GameObjectExtensions.GetGlobalBounds(currentGameObject).center;

						Debug.Log("GenerateIcons - " + currentItem + " prepared");
						if(!stepByStep)
							SaveIt();
					}catch{
						Error ("GenerateIcons - " + currentItem + " : Problem processing this object" );
						if(currentGameObject != null) DestroyImmediate(currentGameObject.gameObject);
						currentItem++;
						okItem ++;
					}
				}
				else{
					EndGeneration();
				}
			}
		}

		public void SaveIt(){
			WindowInfo game = new WindowInfo("UnityEditor.GameView", "Game", "Window/Game");
			game.FindFirstOrCreate().Focus();
			StartCoroutine(MakeIcon());
		}

		IEnumerator MakeIcon() {
			yield return new WaitForEndOfFrame();
			if (generateItems) {
				if(currentGameObject != null){
					string path = iconOutputFolder;
					try{
						Texture2D tex = new Texture2D(iconSize, iconSize);
						tex.ReadPixels(new Rect(Screen.width*0.5f-iconSize*0.5f,Screen.height*0.5f-iconSize*0.5f,(float)iconSize,(float)iconSize), 0, 0);
						tex.Apply();
						if(tex != null){
							// choosing path :
							if(path == "") 
								path = EditorTools.GetResourcesPath(completeQueue[currentItem]);
							else{
								if(! path.StartsWith("/")) path = "/" + path;
								path = Application.dataPath + path;
							}
							if(!path.EndsWith("/")) path += "/";
							path += prefix + completeQueue[currentItem].name + suffix + ".png";

							// Saving picture
							EditorTools.SaveTextToPath(path, tex);
							Debug.Log("GenerateIcons - " + currentItem + " - icon at " + path);
						}
					}catch{
						Error("GenerateIcons - " + currentItem + " - icon can not be saved. Check your export path : " + path);
					}
					// Finnaly, destroy the actual GameObject :
					 DestroyImmediate(currentGameObject.gameObject);
				}
				currentItem++;
				okItem ++;
				doStep = true;
			}
		}

		public void SwitchUI(){
			useUI = ! useUI;
			Canvas[] canvas = GetComponentsInChildren<Canvas>(true);
			foreach(Canvas canv in canvas){
				canv.gameObject.SetActive(useUI);
			}
		}


		Material maskMat;
		void OnRenderImage(RenderTexture src, RenderTexture dest) {
			if(iconMask != null){
				if(maskMat == null){
					maskMat = new Material(Shader.Find("J4F/Unlit/AlphaMask"));
				}
				maskMat.SetTexture("_AlphaTex",iconMask);
				Graphics.Blit(src, dest, maskMat);
			}else{
				Graphics.Blit(src, dest);
			}
		}
		
		/// <summary>
		/// Make the cam allways looking to 0,0,0
		/// </summary>
		void Update(){
			// Lookat world center
			transform.LookAt(Vector3.zero);
			// resize viewport
			if(generationCam == null) generationCam = GetComponent<Camera>(); 
			generationCam.pixelRect = new Rect(Screen.width*0.5f-iconSize*0.5f,Screen.height*0.5f-iconSize*0.5f,(float)iconSize,(float)iconSize);
			generationCam.orthographicSize = cameraSize;
		}
		
		/*
		void OnDrawGizmos() {
			Color gizColor = new Color(1,1,1,0.1f);
			if(boudingBoxZone != null && boudingBoxZone.activeSelf){
				Gizmos.color = gizColor;
				Bounds bounds = GameObjectExtensions.GetGlobalBounds(boudingBoxZone);
				Gizmos.DrawCube(bounds.center, bounds.size);
			}
		}*/
		
		/// <summary>
		/// translate the GO bounds to have the size in screen
		/// </summary>
		/// <returns>The to screen.</returns>
		/// <param name="bounds">Bounds.</param>
		protected Rect BoundsToScreen(Bounds bounds)
		{
			Vector3 cen = bounds.center;
			Vector3 ext = bounds.extents;
			Vector2[] extentPoints = new Vector2[8]
			{
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
				
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
				HandleUtility.WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
			};
			
			Vector2 min = extentPoints[0];
			Vector2 max = extentPoints[0];
			
			foreach(Vector2 v in extentPoints)
			{
				min = Vector2.Min(min, v);
				max = Vector2.Max(max, v);
			}
			
			return new Rect(min.x, min.y, max.x-min.x, max.y-min.y);
		}
		void Error(string msg){
			
			Debug.LogError(msg);
			errorStack +="\n" + msg;
		}

		void OnEnable () {
			generateItems = false;
		}
	}














}
#endif