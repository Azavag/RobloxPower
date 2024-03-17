using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace J4F{
	public static class GameObjectExtensions
	{
		public static void ClearChildren (Transform transform)
		{
			var children = new List<GameObject>();
			foreach (Transform child in transform) children.Add(child.gameObject);
			children.ForEach(child => GameObject.Destroy(child));
		}

		/// <summary>
		/// Returns all monobehaviours (casted to T)
		/// </summary>
		/// <typeparam name="T">interface type</typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T[] GetInterfaces<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
			var mObjs = gObj.GetComponents<MonoBehaviour>();
			
			return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
		}
		
		/// <summary>
		/// Returns the first monobehaviour that is of the interface type (casted to T)
		/// </summary>
		/// <typeparam name="T">Interface type</typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T GetInterface<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
			return gObj.GetInterfaces<T>().FirstOrDefault();
		}
		
		/// <summary>
		/// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T GetInterfaceInChildren<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
			return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
		}
		
		/// <summary>
		/// Gets all monobehaviours in children that implement the interface of type T (casted to T)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
			
			var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();
			
			return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
		}

		/// <summary>
		/// Gets the global bounds and center go
		/// </summary>
		/// <returns>The bounds and center.</returns>
		/// <param name="go">Go.</param>
		public static Bounds GetGlobalBounds (this GameObject go){
			var renderers = go.GetComponentsInChildren<Renderer>();
			Bounds bounds = new Bounds();
			foreach(Renderer rend in renderers) { 
				bounds.Encapsulate(rend.bounds);
			}
			return bounds;
		}
	}
}