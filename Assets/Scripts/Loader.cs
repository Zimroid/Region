using UnityEngine;
using System.Collections;

namespace Completed
{   
	public class Loader : MonoBehaviour 
	{
		// Map Manager prefabs
		public GameObject mapManager;


		void Awake ()
		{
			// Instatiate mapManager
			if (MapManager.instance == null){
				Instantiate(mapManager);
			}
		}
	}
}