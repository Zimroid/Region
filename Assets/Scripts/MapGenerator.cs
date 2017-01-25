using UnityEngine;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	//Determine the percentage of land
	[Range(0, 100)]
	public int islandPercent;
	//Determine the size of the beach
	[Range(0, 5)]
	public int beachSize;
	//Determine the size of the beach (transform land close to the map's limit into sand)
	[Range(0, 5)]
	public int border;
	//height of the map
	public static int height = 120;
	//width of the map
	public static int width = 120;
	//The seed of the map
	public string seed;
	//Used for randomize the seed
	public bool useRandomSeed;

	//block of grass (prefabs)
	public GameObject grass;
	//block of sand (prefabs)
	public GameObject sand;
	//block of water (prefabs)
	public GameObject water;
	//block of rock (prefabs)
	public GameObject rock;

	//used to contain all created object here to have a clean project
	private Transform mapHolder;
	private Transform level1Holder;
	private Transform level2Holder;
	private Transform level3Holder;
	//the map, 0 = water, 1 = grass, 2 = sand, 3 = rock
	public static int[,,] map;

	// used to create the map (map renderer)
	public void mapSetup()
	{
		//RandomGenerator will fill map with int
		RandomGenerator ();
		mapHolder = new GameObject ("Map").transform;
		level1Holder = new GameObject ("Level1").transform;
		level1Holder.transform.SetParent (mapHolder);
		level2Holder = new GameObject ("Level2").transform;
		level2Holder.transform.SetParent (mapHolder);
		level3Holder = new GameObject ("Level3").transform;
		level3Holder.transform.SetParent (mapHolder);

		//We create a new block corresponding of each int in map at the right place
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				// by default grass
				GameObject toInit = grass;

				if(map[i,j,0] == 0){
					toInit = water;
				}
				else if(map[i,j,0] == 2){
					toInit = sand;
				}
				else if(map[i,j,0] == 3){
					toInit = rock;
				}

				GameObject instance = Instantiate (toInit, new Vector3 (i-width/2, j-height/2, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (level1Holder);
				SpriteRenderer sprite = instance.GetComponent<SpriteRenderer> ();
				sprite.sortingOrder = 0;
				sprite.sortingLayerName = "Level1";

				if(map[i,j,1] == 1 && j+1 < height){
					toInit = grass;
					instance = Instantiate (toInit, new Vector3 (i-width/2, j+1-height/2, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (level2Holder);
					sprite = instance.GetComponent<SpriteRenderer> ();
					sprite.sortingOrder = 10;
					sprite.sortingLayerName = "Level2";
					sprite.color = new Color(0.8f, 0.8f, 0.8f);
					if (j - 1 > 0 && map [i, j - 1, 1] == -1) {
						toInit = rock;
						instance = Instantiate (toInit, new Vector3 (i-width/2, j-height/2, 0f), Quaternion.identity) as GameObject;
						instance.transform.SetParent (level2Holder);
						sprite = instance.GetComponent<SpriteRenderer> ();
						sprite.sortingOrder = 10;
						sprite.sortingLayerName = "Level2";
					}
				}

				if(map[i,j,2] == 1 && j+2 < height){
					toInit = grass;
					instance = Instantiate (toInit, new Vector3 (i-width/2, j+2-height/2, 0f), Quaternion.identity) as GameObject;
					instance.transform.SetParent (level3Holder);
					sprite = instance.GetComponent<SpriteRenderer> ();
					sprite.sortingOrder = 20;
					sprite.sortingLayerName = "Level3";
					sprite.color = new Color(0.5f, 0.5f, 0.5f);
					if (j - 1 > 0 && map [i, j - 1,2] == -1) {
						toInit = rock;
						instance = Instantiate (toInit, new Vector3 (i-width/2, j+1-height/2, 0f), Quaternion.identity) as GameObject;
						instance.transform.SetParent (level3Holder);
						sprite = instance.GetComponent<SpriteRenderer> ();
						sprite.sortingOrder = 20;
						sprite.sortingLayerName = "Level3";
					}
				}
			}
		}
	}

	//fill the map with appropriate int (level terrain generator)
	void RandomGenerator(){
		// If we want to use a random seed we take the time for seed
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}

		//map initialisation
		map = new int[width, height, 3];

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				//get a random number between 0 and 100 with perlin noise
				float rng = GetRng (i, j);
				//if the point is close to the center (to have more chances to create a good shape for island)
				//and rng is small enough to be less than the percent of land we want
				//then this place will be classic land (we reserve a place for the beach)
				if (rng * DistanceFromCenter (i, j) < islandPercent-beachSize) {
					if (rng < 50) {
						//level 1
						map [i, j, 0] = 1;
						map [i, j, 1] = -1;
						map [i, j, 2] = -1;
					} else if (rng < 70) {
						//level 2
						map [i, j, 0] = 3;
						map [i, j, 1] = 1;
						map [i, j, 2] = -1;
					} else {
						//level 3
						map [i, j, 0] = 3;
						map [i, j, 1] = 3;
						map [i, j, 2] = 1;
					}
				//we extend a little the land and add a beach
				} else if (rng * DistanceFromCenter (i, j) < islandPercent) {
					map [i, j, 0] = 2;
					map [i, j, 1] = -1;
					map [i, j, 2] = -1;
				//the rest of the map will be filled with water
				} else {
					map [i, j, 0] = 0;
					map [i, j, 1] = -1;
					map [i, j, 2] = -1;
				}
				//For the land close to the border of the map
				if (i <= 0+border || i >= width - 1 - border || j <= 0+border || j >= height - 1 - border) {
					//the border will always be water
					if (i == 0 || i == width - 1 || j == 0 || j == height - 1) {
						map [i, j, 0] = 0;
						map [i, j, 1] = -1;
						map [i, j, 2] = -1;
					// for land close to the border we will smooth the coast to be less artificially cut
					} else if (rng * DistanceFromCenter (i, j) < islandPercent){
						map [i, j, 0] = SmoothCoast (i, j);
						map [i, j, 1] = -1;
						map [i, j, 2] = -1;
					}
				}
			}
		}
	}

	// return a float between 0 and 100 using perlin noise
	float GetRng(int i, int j){
		return Mathf.PerlinNoise ((i + seed.GetHashCode ()) / 20f, (j + seed.GetHashCode ()) / 20f) * 100;
	}

	// return a float between 0 and 2, and determine if a point is close to the center of the map or not
	// 0 = close to the center
	// 2 = close to the border
	float DistanceFromCenter(int gridX, int gridY){
		float x = (float)gridX;
		float y = (float)gridY;
		// rapport can be between 0 and 2... we remove 1 so when the distance is 0 it's close to the center !
		float dx = x / (width/2) - 1;
		float dy = y / (height/2) - 1;
		return dx*dx + dy*dy;
	}

	// return an int a give another type for the point given
	int SmoothCoast(int i, int j){
		//we count the number of water close to the given point
		int countProximitedWater = 0;
		for (int k = i - 1; k <= i+1; k++) {
			for (int l = j - 1; l <= j+1; l++) {
				if (k >= 0 && k <= width && l >= 0 && l <= height && (k != i && l != j)) {
					if (k == 0 || k == width || l == 0 || l == height || GetRng (k, l) * DistanceFromCenter (k, l) > islandPercent) {
						countProximitedWater = countProximitedWater + 1;
					}
				}
			}
		}
		// if we have 3 or more water close to the point we convert this point into water
		if (countProximitedWater > 2) {
			return 0;
		// if we have 0 or less water close to the point we convert this point into grass
		}else if(countProximitedWater == 0){
			return 1;
		}
		// else if we have 2 water close to the point we convert this point into sand
		return 2;
	}
}
