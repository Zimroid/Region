using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;      //Allows us to use SceneManager


namespace Completed
{
	public class Player : MonoBehaviour
	{
		private Rigidbody2D rb;
		private SpriteRenderer sprite;
		private int currentPositionX;
		private int currentPositionY;
		private int currentPositionZ;

		void Start(){
			rb = GetComponent<Rigidbody2D>();
			sprite = GetComponent<SpriteRenderer> ();
			sprite.sortingOrder = 25;
			sprite.sortingLayerName = "Level4";
			currentPositionX = MapGenerator.width / 2;
			currentPositionY = MapGenerator.height / 2;
			currentPositionZ = 2;
			if (MapGenerator.map [currentPositionX, currentPositionY, 2] == -1) {
				sprite.sortingOrder = 15;
				sprite.sortingLayerName = "Level3";
				currentPositionZ = 1;
			}
			if (MapGenerator.map [currentPositionX, currentPositionY, 1] == -1) {
				sprite.sortingOrder = 5;
				sprite.sortingLayerName = "Level2";
				currentPositionZ = 0;
			}
		}

		private void Update ()
		{
			int horizontal = 0;     //Used to store the horizontal move direction.
			int vertical = 0;       //Used to store the vertical move direction.


			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));

			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = (int) (Input.GetAxisRaw ("Vertical"));

			int diffZ = 0;

			if (horizontal < 0 && currentPositionX - 1 >= 0) {
				rb.transform.Translate (new Vector3 (-1, 0));
				diffZ = DifferenceZ (currentPositionZ, currentPositionX - 1, currentPositionY);
				rb.transform.Translate (new Vector3 (0, diffZ));
				currentPositionZ = currentPositionZ + diffZ;
				sprite.sortingOrder = currentPositionZ * 10 + 5;
				currentPositionX = currentPositionX - 1;
			}
			if (horizontal > 0 && currentPositionX + 1 < MapGenerator.width) {
				rb.transform.Translate (new Vector3 (1, 0));
				diffZ = DifferenceZ (currentPositionZ, currentPositionX + 1, currentPositionY);
				rb.transform.Translate (new Vector3 (0, diffZ));
				currentPositionZ = currentPositionZ + diffZ;
				sprite.sortingOrder = currentPositionZ * 10 + 5;
				currentPositionX = currentPositionX + 1;
			}
			if (vertical < 0 && currentPositionY - 1 >= 0) {
				rb.transform.Translate (new Vector3 (0, -1));
				diffZ = DifferenceZ (currentPositionZ, currentPositionX, currentPositionY - 1);
				rb.transform.Translate (new Vector3 (0, diffZ));
				currentPositionZ = currentPositionZ + diffZ;
				sprite.sortingOrder = currentPositionZ * 10 + 5;
				currentPositionY = currentPositionY - 1;
			}
			if (vertical > 0 && currentPositionY + 1 < MapGenerator.height) {
				rb.transform.Translate (new Vector3 (0, 1));
				diffZ = DifferenceZ (currentPositionZ, currentPositionX, currentPositionY + 1);
				rb.transform.Translate (new Vector3 (0, diffZ));
				currentPositionZ = currentPositionZ + diffZ;
				sprite.sortingOrder = currentPositionZ * 10 + 5;
				currentPositionY = currentPositionY + 1;
			}
		}

		private int DifferenceZ(int currentPositionZ, int nextX, int nextY){
			int nextZ = 2;
			if (MapGenerator.map [nextX, nextY, 2] == -1) {
				nextZ = 1;
			}
			if (MapGenerator.map [nextX, nextY, 1] == -1) {
				nextZ = 0;
			}
			return nextZ - currentPositionZ;
		}
	}
}