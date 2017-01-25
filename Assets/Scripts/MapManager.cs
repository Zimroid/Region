using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	// Instance of MapManager
	public static MapManager instance = null;
	public MapGenerator map;


	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
		map = GetComponent<MapGenerator> ();
		InitGame ();
	}

	void InitGame()
	{
		map.mapSetup ();
	}
}
