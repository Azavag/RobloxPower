using UnityEngine;
using System.Collections;

namespace J4F{
	public class J4FBehaviour : MonoBehaviour
	{
		void Awake(){
			OnAwake();
		}
		protected virtual void OnAwake(){
		}

		void Start(){
			OnStart();
		}
		protected virtual void OnStart(){
		}

		void Update(){
			OnUpdate();
		}
		protected virtual void OnUpdate(){
		}

	}
}
